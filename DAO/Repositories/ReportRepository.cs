using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using DAO.Helpers;
using DOMAIN.Contracts;
using DOMAIN.Entities.Reports;
using DOMAIN.Enums;

namespace DAO.Repositories
{
    public class ReportRepository : IReportRepository
    {
        // Report 1: Top Products
        public List<TopProductsReportDTO> GetTopProductsReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null, 
            int? topN = null,
            string orderBy = "units")
        {
            var reports = new List<TopProductsReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH ProductSales AS (
                        SELECT 
                            p.ProductId,
                            p.SKU,
                            p.Name AS ProductName,
                            p.Category,
                            SUM(sl.Quantity) AS UnitsSold,
                            SUM(sl.LineTotal) AS Revenue,
                            p.UnitPrice AS ListPrice,
                            AVG(sl.UnitPrice) AS AverageSalePrice
                        FROM SaleLines sl
                        INNER JOIN Products p ON sl.ProductId = p.ProductId
                        INNER JOIN Sales s ON sl.SaleId = s.SaleId
                        WHERE s.IsActive = 1
                            {0}
                            {1}
                        GROUP BY p.ProductId, p.SKU, p.Name, p.Category, p.UnitPrice
                    )
                    SELECT 
                        ROW_NUMBER() OVER (ORDER BY {2} DESC) AS Ranking,
                        SKU,
                        ProductName,
                        Category,
                        UnitsSold,
                        Revenue,
                        ListPrice,
                        AverageSalePrice
                    FROM ProductSales
                    ORDER BY {2} DESC
                    {3}";

                var dateFilter = "";
                var categoryFilter = "";
                var orderByClause = orderBy == "revenue" ? "Revenue" : "UnitsSold";
                var topNClause = topN.HasValue ? $"OFFSET 0 ROWS FETCH NEXT {topN.Value} ROWS ONLY" : "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    categoryFilter = "AND p.Category = @Category";
                }

                var finalQuery = string.Format(query, dateFilter, categoryFilter, orderByClause, topNClause);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (!string.IsNullOrEmpty(category))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new TopProductsReportDTO
                            {
                                Ranking = reader.GetInt32(reader.GetOrdinal("Ranking")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? "" : reader.GetString(reader.GetOrdinal("Category")),
                                UnitsSold = reader.GetInt32(reader.GetOrdinal("UnitsSold")),
                                Revenue = reader.GetDecimal(reader.GetOrdinal("Revenue")),
                                ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                                AverageSalePrice = reader.GetDecimal(reader.GetOrdinal("AverageSalePrice"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 2: Client Purchases
        public List<ClientPurchasesReportDTO> GetClientPurchasesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? clientId = null,
            int? topN = null)
        {
            var reports = new List<ClientPurchasesReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    SELECT 
                        c.ClientId,
                        c.Nombre + ' ' + c.Apellido AS ClientFullName,
                        c.DNI,
                        c.Correo AS Email,
                        COUNT(DISTINCT s.SaleId) AS PurchaseCount,
                        SUM(s.TotalAmount) AS TotalSpent,
                        SUM(sl.Quantity) AS TotalUnits,
                        COUNT(DISTINCT sl.ProductId) AS DistinctProducts,
                        AVG(s.TotalAmount) AS AverageTicket
                    FROM Clients c
                    INNER JOIN Sales s ON c.ClientId = s.ClientId
                    INNER JOIN SaleLines sl ON s.SaleId = sl.SaleId
                    WHERE s.IsActive = 1 AND c.IsActive = 1
                        {0}
                        {1}
                    GROUP BY c.ClientId, c.Nombre, c.Apellido, c.DNI, c.Correo
                    ORDER BY TotalSpent DESC
                    {2}";

                var dateFilter = "";
                var clientFilter = "";
                var topNClause = topN.HasValue ? $"OFFSET 0 ROWS FETCH NEXT {topN.Value} ROWS ONLY" : "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (clientId.HasValue)
                {
                    clientFilter = "AND c.ClientId = @ClientId";
                }

                var finalQuery = string.Format(query, dateFilter, clientFilter, topNClause);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (clientId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", clientId.Value));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new ClientPurchasesReportDTO
                            {
                                ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                ClientFullName = reader.GetString(reader.GetOrdinal("ClientFullName")),
                                DNI = reader.IsDBNull(reader.GetOrdinal("DNI")) ? "" : reader.GetString(reader.GetOrdinal("DNI")),
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString(reader.GetOrdinal("Email")),
                                PurchaseCount = reader.GetInt32(reader.GetOrdinal("PurchaseCount")),
                                TotalSpent = reader.GetDecimal(reader.GetOrdinal("TotalSpent")),
                                TotalUnits = reader.GetInt32(reader.GetOrdinal("TotalUnits")),
                                DistinctProducts = reader.GetInt32(reader.GetOrdinal("DistinctProducts")),
                                AverageTicket = reader.GetDecimal(reader.GetOrdinal("AverageTicket")),
                                ProductDetails = new List<ClientProductDetail>()
                            });
                        }
                    }
                }

                // Load product details for each client
                foreach (var report in reports)
                {
                    report.ProductDetails = GetClientProductDetails(report.ClientId, startDate, endDate);
                }
            }
            return reports;
        }

        private List<ClientProductDetail> GetClientProductDetails(int clientId, DateTime? startDate, DateTime? endDate)
        {
            var details = new List<ClientProductDetail>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    SELECT 
                        p.Name AS ProductName,
                        p.SKU,
                        SUM(sl.Quantity) AS Quantity,
                        SUM(sl.LineTotal) AS TotalAmount
                    FROM SaleLines sl
                    INNER JOIN Products p ON sl.ProductId = p.ProductId
                    INNER JOIN Sales s ON sl.SaleId = s.SaleId
                    WHERE s.ClientId = @ClientId AND s.IsActive = 1
                        {0}
                    GROUP BY p.Name, p.SKU
                    ORDER BY TotalAmount DESC";

                var dateFilter = "";
                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }

                var finalQuery = string.Format(query, dateFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", clientId));
                    
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            details.Add(new ClientProductDetail
                            {
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"))
                            });
                        }
                    }
                }
            }
            return details;
        }

        // Report 3: Price Variation
        public List<PriceVariationReportDTO> GetPriceVariationReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null)
        {
            var reports = new List<PriceVariationReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    SELECT 
                        p.SKU,
                        p.Name AS ProductName,
                        p.Category,
                        p.UnitPrice AS ListPrice,
                        MIN(sl.UnitPrice) AS MinSalePrice,
                        MAX(sl.UnitPrice) AS MaxSalePrice,
                        AVG(sl.UnitPrice) AS AverageSalePrice,
                        (AVG(sl.UnitPrice) - p.UnitPrice) AS AbsoluteVariation,
                        CASE 
                            WHEN p.UnitPrice > 0 THEN ((AVG(sl.UnitPrice) - p.UnitPrice) / p.UnitPrice * 100)
                            ELSE 0
                        END AS PercentageVariation
                    FROM Products p
                    INNER JOIN SaleLines sl ON p.ProductId = sl.ProductId
                    INNER JOIN Sales s ON sl.SaleId = s.SaleId
                    WHERE s.IsActive = 1 AND p.IsActive = 1
                        {0}
                        {1}
                        {2}
                    GROUP BY p.SKU, p.Name, p.Category, p.UnitPrice
                    ORDER BY p.SKU";

                var dateFilter = "";
                var productFilter = "";
                var categoryFilter = "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (productId.HasValue)
                {
                    productFilter = "AND p.ProductId = @ProductId";
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    categoryFilter = "AND p.Category = @Category";
                }

                var finalQuery = string.Format(query, dateFilter, productFilter, categoryFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (productId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId.Value));
                    }
                    
                    if (!string.IsNullOrEmpty(category))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new PriceVariationReportDTO
                            {
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? "" : reader.GetString(reader.GetOrdinal("Category")),
                                ListPrice = reader.GetDecimal(reader.GetOrdinal("ListPrice")),
                                MinSalePrice = reader.GetDecimal(reader.GetOrdinal("MinSalePrice")),
                                MaxSalePrice = reader.GetDecimal(reader.GetOrdinal("MaxSalePrice")),
                                AverageSalePrice = reader.GetDecimal(reader.GetOrdinal("AverageSalePrice")),
                                AbsoluteVariation = reader.GetDecimal(reader.GetOrdinal("AbsoluteVariation")),
                                PercentageVariation = reader.GetDecimal(reader.GetOrdinal("PercentageVariation"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 4: Seller Performance
        public List<SellerPerformanceReportDTO> GetSellerPerformanceReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string sellerName = null,
            string category = null)
        {
            var reports = new List<SellerPerformanceReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH SellerStats AS (
                        SELECT 
                            s.SellerName,
                            COUNT(DISTINCT s.SaleId) AS TotalSales,
                            SUM(sl.Quantity) AS TotalUnits,
                            SUM(s.TotalAmount) AS TotalRevenue,
                            AVG(s.TotalAmount) AS AverageTicket
                        FROM Sales s
                        INNER JOIN SaleLines sl ON s.SaleId = sl.SaleId
                        INNER JOIN Products p ON sl.ProductId = p.ProductId
                        WHERE s.IsActive = 1
                            {0}
                            {1}
                            {2}
                        GROUP BY s.SellerName
                    ),
                    TopProducts AS (
                        SELECT 
                            s.SellerName,
                            p.Name AS ProductName,
                            SUM(sl.Quantity) AS ProductQuantity,
                            ROW_NUMBER() OVER (PARTITION BY s.SellerName ORDER BY SUM(sl.Quantity) DESC) AS RowNum
                        FROM Sales s
                        INNER JOIN SaleLines sl ON s.SaleId = sl.SaleId
                        INNER JOIN Products p ON sl.ProductId = p.ProductId
                        WHERE s.IsActive = 1
                            {0}
                            {1}
                            {2}
                        GROUP BY s.SellerName, p.Name
                    )
                    SELECT 
                        ss.SellerName,
                        ss.TotalSales,
                        ss.TotalUnits,
                        ss.TotalRevenue,
                        ss.AverageTicket,
                        ISNULL(tp.ProductName, '') AS TopProduct,
                        ISNULL(tp.ProductQuantity, 0) AS TopProductQuantity
                    FROM SellerStats ss
                    LEFT JOIN TopProducts tp ON ss.SellerName = tp.SellerName AND tp.RowNum = 1
                    ORDER BY ss.TotalRevenue DESC";

                var dateFilter = "";
                var sellerFilter = "";
                var categoryFilter = "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (!string.IsNullOrEmpty(sellerName))
                {
                    sellerFilter = "AND s.SellerName LIKE @SellerName";
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    categoryFilter = "AND p.Category = @Category";
                }

                var finalQuery = string.Format(query, dateFilter, sellerFilter, categoryFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (!string.IsNullOrEmpty(sellerName))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@SellerName", "%" + sellerName + "%"));
                    }
                    
                    if (!string.IsNullOrEmpty(category))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new SellerPerformanceReportDTO
                            {
                                SellerName = reader.GetString(reader.GetOrdinal("SellerName")),
                                TotalSales = reader.GetInt32(reader.GetOrdinal("TotalSales")),
                                TotalUnits = reader.GetInt32(reader.GetOrdinal("TotalUnits")),
                                TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                AverageTicket = reader.GetDecimal(reader.GetOrdinal("AverageTicket")),
                                TopProduct = reader.GetString(reader.GetOrdinal("TopProduct")),
                                TopProductQuantity = reader.GetInt32(reader.GetOrdinal("TopProductQuantity"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 5: Category Sales
        public List<CategorySalesReportDTO> GetCategorySalesReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string category = null)
        {
            var reports = new List<CategorySalesReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH CategoryTotals AS (
                        SELECT 
                            ISNULL(p.Category, 'Sin Categoría') AS Category,
                            SUM(sl.Quantity) AS UnitsSold,
                            SUM(sl.LineTotal) AS TotalRevenue
                        FROM SaleLines sl
                        INNER JOIN Products p ON sl.ProductId = p.ProductId
                        INNER JOIN Sales s ON sl.SaleId = s.SaleId
                        WHERE s.IsActive = 1
                            {0}
                            {1}
                        GROUP BY p.Category
                    ),
                    GrandTotal AS (
                        SELECT SUM(TotalRevenue) AS GrandTotalRevenue
                        FROM CategoryTotals
                    )
                    SELECT 
                        ct.Category,
                        ct.UnitsSold,
                        ct.TotalRevenue,
                        CASE 
                            WHEN gt.GrandTotalRevenue > 0 THEN (ct.TotalRevenue / gt.GrandTotalRevenue * 100)
                            ELSE 0
                        END AS PercentageOfTotal
                    FROM CategoryTotals ct
                    CROSS JOIN GrandTotal gt
                    ORDER BY ct.TotalRevenue DESC";

                var dateFilter = "";
                var categoryFilter = "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    categoryFilter = "AND p.Category = @Category";
                }

                var finalQuery = string.Format(query, dateFilter, categoryFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (!string.IsNullOrEmpty(category))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new CategorySalesReportDTO
                            {
                                Category = reader.GetString(reader.GetOrdinal("Category")),
                                UnitsSold = reader.GetInt32(reader.GetOrdinal("UnitsSold")),
                                TotalRevenue = reader.GetDecimal(reader.GetOrdinal("TotalRevenue")),
                                PercentageOfTotal = reader.GetDecimal(reader.GetOrdinal("PercentageOfTotal"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 6: Revenue by Date
        public List<RevenueByDateReportDTO> GetRevenueByDateReport(
            DateTime? startDate, 
            DateTime? endDate, 
            string movementType = null,
            int? warehouseId = null)
        {
            var reports = new List<RevenueByDateReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH DailyRevenue AS (
                        SELECT 
                            CAST(s.SaleDate AS DATE) AS ReportDate,
                            SUM(s.TotalAmount) AS SalesRevenue
                        FROM Sales s
                        WHERE s.IsActive = 1
                            {0}
                        GROUP BY CAST(s.SaleDate AS DATE)
                    ),
                    DailyMovements AS (
                        SELECT 
                            CAST(sm.MovementDate AS DATE) AS ReportDate,
                            SUM(CASE WHEN sm.MovementType = 0 THEN 1 ELSE 0 END) AS StockInMovements,
                            SUM(CASE WHEN sm.MovementType = 0 THEN sml.Quantity ELSE 0 END) AS StockInUnits,
                            SUM(CASE WHEN sm.MovementType = 1 THEN 1 ELSE 0 END) AS StockOutMovements,
                            SUM(CASE WHEN sm.MovementType = 1 THEN sml.Quantity ELSE 0 END) AS StockOutUnits
                        FROM StockMovements sm
                        INNER JOIN StockMovementLines sml ON sm.MovementId = sml.MovementId
                        WHERE 1=1
                            {1}
                            {2}
                            {3}
                        GROUP BY CAST(sm.MovementDate AS DATE)
                    )
                    SELECT 
                        ISNULL(dr.ReportDate, dm.ReportDate) AS ReportDate,
                        ISNULL(dr.SalesRevenue, 0) AS SalesRevenue,
                        ISNULL(dm.StockInMovements, 0) AS StockInMovements,
                        ISNULL(dm.StockInUnits, 0) AS StockInUnits,
                        ISNULL(dm.StockOutMovements, 0) AS StockOutMovements,
                        ISNULL(dm.StockOutUnits, 0) AS StockOutUnits
                    FROM DailyRevenue dr
                    FULL OUTER JOIN DailyMovements dm ON dr.ReportDate = dm.ReportDate
                    ORDER BY ISNULL(dr.ReportDate, dm.ReportDate)";

                var dateFilter = "";
                var movementTypeFilter = "";
                var warehouseFilter = "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND CAST(s.SaleDate AS DATE) >= @StartDate AND CAST(s.SaleDate AS DATE) <= @EndDate";
                }
                
                var dateFilterMovements = "";
                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilterMovements = "AND CAST(sm.MovementDate AS DATE) >= @StartDate AND CAST(sm.MovementDate AS DATE) <= @EndDate";
                }
                
                if (!string.IsNullOrEmpty(movementType))
                {
                    int? typeValue = null;
                    switch (movementType.ToLower())
                    {
                        case "in":
                            typeValue = 0;
                            break;
                        case "out":
                            typeValue = 1;
                            break;
                        case "transfer":
                            typeValue = 2;
                            break;
                        case "adjustment":
                            typeValue = 3;
                            break;
                    }
                    
                    if (typeValue.HasValue)
                    {
                        movementTypeFilter = "AND sm.MovementType = @MovementType";
                    }
                }
                
                if (warehouseId.HasValue)
                {
                    warehouseFilter = "AND (sm.SourceWarehouseId = @WarehouseId OR sm.DestinationWarehouseId = @WarehouseId)";
                }

                var finalQuery = string.Format(query, dateFilter, dateFilterMovements, movementTypeFilter, warehouseFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value.Date));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value.Date));
                    }
                    
                    if (!string.IsNullOrEmpty(movementType) && movementTypeFilter != "")
                    {
                        int typeValue = 0;
                        switch (movementType.ToLower())
                        {
                            case "in":
                                typeValue = 0;
                                break;
                            case "out":
                                typeValue = 1;
                                break;
                            case "transfer":
                                typeValue = 2;
                                break;
                            case "adjustment":
                                typeValue = 3;
                                break;
                        }
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@MovementType", typeValue));
                    }
                    
                    if (warehouseId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@WarehouseId", warehouseId.Value));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new RevenueByDateReportDTO
                            {
                                ReportDate = reader.GetDateTime(reader.GetOrdinal("ReportDate")),
                                SalesRevenue = reader.GetDecimal(reader.GetOrdinal("SalesRevenue")),
                                StockInMovements = reader.GetInt32(reader.GetOrdinal("StockInMovements")),
                                StockInUnits = reader.GetInt32(reader.GetOrdinal("StockInUnits")),
                                StockOutMovements = reader.GetInt32(reader.GetOrdinal("StockOutMovements")),
                                StockOutUnits = reader.GetInt32(reader.GetOrdinal("StockOutUnits"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 7: Client Product Ranking
        public List<ClientProductRankingReportDTO> GetClientProductRankingReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? productId = null, 
            string category = null,
            int? topN = null)
        {
            var reports = new List<ClientProductRankingReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH ClientProductSales AS (
                        SELECT 
                            c.ClientId,
                            c.Nombre + ' ' + c.Apellido AS ClientFullName,
                            c.DNI,
                            p.Name AS ProductName,
                            p.SKU,
                            p.Category,
                            SUM(sl.Quantity) AS UnitsPurchased,
                            SUM(sl.LineTotal) AS TotalSpent
                        FROM Clients c
                        INNER JOIN Sales s ON c.ClientId = s.ClientId
                        INNER JOIN SaleLines sl ON s.SaleId = sl.SaleId
                        INNER JOIN Products p ON sl.ProductId = p.ProductId
                        WHERE s.IsActive = 1 AND c.IsActive = 1
                            {0}
                            {1}
                            {2}
                        GROUP BY c.ClientId, c.Nombre, c.Apellido, c.DNI, p.Name, p.SKU, p.Category
                    ),
                    ProductTotals AS (
                        SELECT 
                            ProductName,
                            SUM(UnitsPurchased) AS TotalProductSales
                        FROM ClientProductSales
                        GROUP BY ProductName
                    )
                    SELECT 
                        cps.ClientId,
                        cps.ClientFullName,
                        cps.DNI,
                        cps.ProductName,
                        cps.SKU,
                        cps.Category,
                        cps.UnitsPurchased,
                        cps.TotalSpent,
                        CASE 
                            WHEN pt.TotalProductSales > 0 THEN (cps.UnitsPurchased * 100.0 / pt.TotalProductSales)
                            ELSE 0
                        END AS PercentageOfProductSales
                    FROM ClientProductSales cps
                    INNER JOIN ProductTotals pt ON cps.ProductName = pt.ProductName
                    ORDER BY cps.TotalSpent DESC
                    {3}";

                var dateFilter = "";
                var productFilter = "";
                var categoryFilter = "";
                var topNClause = topN.HasValue ? $"OFFSET 0 ROWS FETCH NEXT {topN.Value} ROWS ONLY" : "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (productId.HasValue)
                {
                    productFilter = "AND p.ProductId = @ProductId";
                }
                
                if (!string.IsNullOrEmpty(category))
                {
                    categoryFilter = "AND p.Category = @Category";
                }

                var finalQuery = string.Format(query, dateFilter, productFilter, categoryFilter, topNClause);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (productId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ProductId", productId.Value));
                    }
                    
                    if (!string.IsNullOrEmpty(category))
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@Category", category));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new ClientProductRankingReportDTO
                            {
                                ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                ClientFullName = reader.GetString(reader.GetOrdinal("ClientFullName")),
                                DNI = reader.IsDBNull(reader.GetOrdinal("DNI")) ? "" : reader.GetString(reader.GetOrdinal("DNI")),
                                ProductName = reader.GetString(reader.GetOrdinal("ProductName")),
                                SKU = reader.GetString(reader.GetOrdinal("SKU")),
                                Category = reader.IsDBNull(reader.GetOrdinal("Category")) ? "" : reader.GetString(reader.GetOrdinal("Category")),
                                UnitsPurchased = reader.GetInt32(reader.GetOrdinal("UnitsPurchased")),
                                TotalSpent = reader.GetDecimal(reader.GetOrdinal("TotalSpent")),
                                PercentageOfProductSales = reader.GetDecimal(reader.GetOrdinal("PercentageOfProductSales"))
                            });
                        }
                    }
                }
            }
            return reports;
        }

        // Report 8: Client Ticket Average
        public List<ClientTicketAverageReportDTO> GetClientTicketAverageReport(
            DateTime? startDate, 
            DateTime? endDate, 
            int? clientId = null,
            int? minPurchases = null)
        {
            var reports = new List<ClientTicketAverageReportDTO>();
            using (var connection = DatabaseHelper.GetConnection())
            {
                var query = @"
                    WITH ClientTickets AS (
                        SELECT 
                            c.ClientId,
                            c.Nombre + ' ' + c.Apellido AS ClientFullName,
                            c.DNI,
                            s.TotalAmount
                        FROM Clients c
                        INNER JOIN Sales s ON c.ClientId = s.ClientId
                        WHERE s.IsActive = 1 AND c.IsActive = 1
                            {0}
                            {1}
                    ),
                    ClientStats AS (
                        SELECT 
                            ClientId,
                            ClientFullName,
                            DNI,
                            COUNT(*) AS PurchaseCount,
                            SUM(TotalAmount) AS TotalSpent,
                            AVG(TotalAmount) AS AverageTicket,
                            MIN(TotalAmount) AS MinTicket,
                            MAX(TotalAmount) AS MaxTicket,
                            STDEV(TotalAmount) AS StdDeviation
                        FROM ClientTickets
                        GROUP BY ClientId, ClientFullName, DNI
                        {2}
                    )
                    SELECT 
                        ClientId,
                        ClientFullName,
                        DNI,
                        PurchaseCount,
                        TotalSpent,
                        AverageTicket,
                        MinTicket,
                        MaxTicket,
                        ISNULL(StdDeviation, 0) AS StdDeviation
                    FROM ClientStats
                    ORDER BY AverageTicket DESC";

                var dateFilter = "";
                var clientFilter = "";
                var minPurchasesFilter = "";

                if (startDate.HasValue && endDate.HasValue)
                {
                    dateFilter = "AND s.SaleDate >= @StartDate AND s.SaleDate <= @EndDate";
                }
                
                if (clientId.HasValue)
                {
                    clientFilter = "AND c.ClientId = @ClientId";
                }
                
                if (minPurchases.HasValue)
                {
                    minPurchasesFilter = "HAVING COUNT(*) >= @MinPurchases";
                }

                var finalQuery = string.Format(query, dateFilter, clientFilter, minPurchasesFilter);

                using (var command = new SqlCommand(finalQuery, connection))
                {
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@StartDate", startDate.Value));
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@EndDate", endDate.Value));
                    }
                    
                    if (clientId.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@ClientId", clientId.Value));
                    }
                    
                    if (minPurchases.HasValue)
                    {
                        command.Parameters.Add(DatabaseHelper.CreateParameter("@MinPurchases", minPurchases.Value));
                    }

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new ClientTicketAverageReportDTO
                            {
                                ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                                ClientFullName = reader.GetString(reader.GetOrdinal("ClientFullName")),
                                DNI = reader.IsDBNull(reader.GetOrdinal("DNI")) ? "" : reader.GetString(reader.GetOrdinal("DNI")),
                                PurchaseCount = reader.GetInt32(reader.GetOrdinal("PurchaseCount")),
                                TotalSpent = reader.GetDecimal(reader.GetOrdinal("TotalSpent")),
                                AverageTicket = reader.GetDecimal(reader.GetOrdinal("AverageTicket")),
                                MinTicket = reader.GetDecimal(reader.GetOrdinal("MinTicket")),
                                MaxTicket = reader.GetDecimal(reader.GetOrdinal("MaxTicket")),
                                StdDeviation = reader.GetDecimal(reader.GetOrdinal("StdDeviation"))
                            });
                        }
                    }
                }
            }
            return reports;
        }
    }
}
