namespace NibulonUrkPosta.Services;

public class AupService(ApplicationDbContext context) : IAupService
{
    public async Task<bool> ImportDataFromExcelAsync(IFormFile file)
    {
        try
        {
            var excelData = await GetDataFromExcelAsync(file);
            var databaseOperationResult = await UploadToDatabaseAsync(excelData);

            return databaseOperationResult;
        }
        catch
        {
            throw;
        }
    }

    public async Task<byte[]> ExportDataToExcelAsync()
    {
        try
        {
            var records = context.PostIndexes.Select(e => new PostIndex()
            {
                PostIndexCode = e.PostIndexCode,
                CityName = e.CityName,
                DistrictName = e.DistrictName,
                RegionName = e.RegionName
            }).ToList();

            return await CreateExcelFileAsync(records);
        }
        catch
        {
            throw;
        }
    }

    public async Task<AupFilterViewModel> GetAupFilterAsync(AupFilterViewModel model, int page)
    {
        try
        {
            int pageSize = 20;

            var query = context.PostIndexes.AsQueryable();

            if (!string.IsNullOrEmpty(model.PostCode))
                query = query.Where(r => r.PostIndexCode.Contains(model.PostCode));

            if (!string.IsNullOrEmpty(model.CityName))
                query = query.Where(r => r.CityName.ToLower().Contains(model.CityName.ToLower()));

            if (model.DistrictId.HasValue)
                query = query.Where(r => r.DistrictId == model.DistrictId);

            if (model.RegionId.HasValue)
                query = query.Where(r => r.RegionId == model.RegionId);

            int totalRecords = await query.CountAsync();
            var records = await query.Skip((page-1) * pageSize).Take(pageSize).ToListAsync();

            var viewModel = new AupFilterViewModel
            {
                PostCode  = model.PostCode,
                CityName = model.CityName,
                DistrictId = model.DistrictId,
                RegionId = model.RegionId,
                Districts = await context.Districts.OrderBy(x => x.DistrictName).ToListAsync(),
                Regions = await context.Regions.OrderBy(x => x.RegionName).ToListAsync(),
                PostCodes = records,
                CurrentPage = (uint)page,
                TotalPages = (uint)Math.Ceiling((double)totalRecords / pageSize)
            };

            return viewModel;
        }
        catch
        {
            throw;
        }
    }
    
    private async Task<byte[]> CreateExcelFileAsync(List<PostIndex> records)
    {
        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    WorkbookPart workbookPart = document.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                    SheetData sheetData = new SheetData();
                    worksheetPart.Worksheet = new Worksheet(sheetData);

                    Sheets sheets = document.WorkbookPart.Workbook.AppendChild(new Sheets());
                    Sheet sheet = new Sheet()
                    {
                        Id = document.WorkbookPart.GetIdOfPart(worksheetPart),
                        SheetId = 1,
                        Name = $"post-codes"
                    };
                    sheets.Append(sheet);
                    AddTitleRow(sheetData);
                    AddRows(sheetData, records);
                    document.WorkbookPart.Workbook.Save();
                }

                return memoryStream.ToArray();
            }
        }
        catch
        {
            throw;
        }
    }

    private void AddTitleRow(SheetData sheetData)
    {
        Row rowForTitle = new Row() { RowIndex = 1 };
        rowForTitle.Append(CreateCell("Поштовий індекс", CellValues.String));
        rowForTitle.Append(CreateCell("Місто", CellValues.String));
        rowForTitle.Append(CreateCell("Район", CellValues.String));
        rowForTitle.Append(CreateCell("Область", CellValues.String));
        sheetData.Append(rowForTitle);
    }

    private void AddRows(SheetData sheetData, List<PostIndex> records)
    {
        try
        {
            uint rowIndex = 2;
            foreach (var record in records)
            {
                Row row = new Row() { RowIndex = rowIndex };
                row.Append(CreateCell(record.PostIndexCode, CellValues.String));
                row.Append(CreateCell(record.CityName, CellValues.String));
                row.Append(CreateCell(record.DistrictName, CellValues.String));
                row.Append(CreateCell(record.RegionName, CellValues.String));
                sheetData.Append(row);
                rowIndex++;
            }
        }
        catch
        {
            throw;
        }
    }
    
    private Cell CreateCell(string text, CellValues dataType)
    {
        var cell = new Cell()
        {
            DataType = new EnumValue<CellValues>(dataType),
            CellValue = new CellValue(text),
        };

        return cell;
    }

    private async Task<bool> UploadToDatabaseAsync(List<ExcelAupDto> excelData)
    {
        try
        {
            var regionsDict = context.Regions.ToDictionary(r => r.RegionName, r => r);
            var districtsDict = context.Districts.ToDictionary(d => d.DistrictName, d => d);
            var citiesDict = context.Cities.ToDictionary(c => c.CityName, c => c);
            var postIndicesDict = context.PostIndexes.ToDictionary(p => p.PostIndexCode, p => p);

            foreach (var item in excelData)
            {
                if (string.IsNullOrWhiteSpace(item.Region) || string.IsNullOrWhiteSpace(item.District) ||
                    string.IsNullOrWhiteSpace(item.City) || string.IsNullOrWhiteSpace(item.PostCode))
                {
                    continue;
                }

                if (!regionsDict.TryGetValue(item.Region, out var region))
                {
                    region = new Region { RegionName = item.Region, Cities = new List<City>() };
                    context.Regions.Add(region);
                    regionsDict[item.Region] = region;
                }

                if (!districtsDict.TryGetValue(item.District, out var district))
                {
                    district = new District { DistrictName = item.District, Cities = new List<City>() };
                    context.Districts.Add(district);
                    districtsDict[item.District] = district;
                }

                if (!citiesDict.TryGetValue(item.City, out var city))
                {
                    city = new City
                    {
                        CityName = item.City,
                        Region = region,
                        District = district,
                        RegionId = region.Id,
                        DistrictId = district.Id
                    };
                    context.Cities.Add(city);
                    citiesDict[item.City] = city;
                }

                if (!postIndicesDict.ContainsKey(item.PostCode))
                {
                    var postIndex = new PostIndex
                    {
                        PostIndexCode = item.PostCode,
                        CityId = city.Id,
                        CityName = city.CityName,
                        RegionId = region.Id,
                        RegionName = region.RegionName,
                        DistrictId = district.Id,
                        DistrictName = district.DistrictName,
                        City = city,
                        Region = region,
                        District = district
                    };
                    context.PostIndexes.Add(postIndex);
                    postIndicesDict[item.PostCode] = postIndex;
                }
            }

            await context.SaveChangesAsync();

            return true;
        }
        catch
        {
            throw;
        }
    }

    private async Task<List<ExcelAupDto>> GetDataFromExcelAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new NoFileSelectedException("File not selected");

        var dataList = new List<ExcelAupDto>();

        try
        {
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int i = 0;
                    do
                    {
                        while (reader.Read())
                        {
                            if (reader.GetValue(1) is null)
                            {
                                continue;
                            }

                            if (i >= 2)
                            {
                                var excelRow = new ExcelAupDto()
                                {
                                    Region = reader.GetValue(1).ToString(),
                                    OldDistrict = (reader.GetValue(2) is null)
                                        ? string.Empty
                                        : reader.GetValue(2).ToString(),
                                    District = reader.GetValue(3).ToString(),
                                    City = reader.GetValue(4).ToString(),
                                    PostCode = reader.GetValue(5).ToString()
                                };
                                dataList.Add(excelRow);
                            }

                            i++;
                        }
                    } while (reader.NextResult());
                }
            }

            return dataList;
        }
        catch
        {
            throw;
        }
    }
}