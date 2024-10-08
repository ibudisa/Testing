USE [BUYDBEST]
GO
/****** Object:  StoredProcedure [dbo].[pr_vendor_BUYDBEST_template_generic_load]    Script Date: 09/24/2012 00:18:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/****** Object:  StoredProcedure [dbo].[pr_vendor_BUYDBEST_template_generic_load]  ********/



ALTER PROCEDURE [dbo].[pr_vendor_BUYDBEST_template_generic_load] @CurrentvendorName Varchar(50),
		@debug CHAR(1) = 'N'
AS

		

BEGIN
   
	SET NOCOUNT ON
	
	Declare @vendorCount int
	BeginBUYDBESTSproc:
	
	
	Set @vendorCount =(Select COUNT(*)  from dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_generic_load is not null)
	If (@vendorCount=0)
	Begin 
	 INSERT INTO dbo.CurrentRunningVendor with(tablock)(pr_vendor_BUYDBEST_template_generic_load) VALUES(@CurrentvendorName )
	
	BEGIN TRY 
	--BEGIN TRAN @CurrentVendorName

	-- Variable declarations
	DECLARE 
		@columnCount INT,
		@Sno INT,
		@sourceField VARCHAR(255),
		@stagingField VARCHAR(70),
		@BUYDBESTField VARCHAR(50),
		@defaultValue VARCHAR(500),
		@tableDefault VARCHAR(500),
		@insertList VARCHAR(MAX),
		@selectList VARCHAR(MAX),
		@whereClause VARCHAR(MAX),
		@sql VARCHAR(MAX),
		@sql2 VARCHAR(MAX),
		@BUYDBESTTemplateStagingTable VARCHAR(255),
		@vendorTable VARCHAR(255),
		@actualvendorTable VARCHAR(255),
		@BUYDBESTTemplateTable VARCHAR(255),
		@defaultBUYDBESTTemplate VARCHAR(100),
		@BUYDBESTTemplate VARCHAR(100),
		@inventoryTable VARCHAR(255),
		@image VARCHAR(255),
		@localImagePath VARCHAR(MAX),
		@localImageFormat VARCHAR(10),
		@minprice NUMERIC(18,2),

		@weight VARCHAR(255),
		@actualWeight VARCHAR(255),
		@courier VARCHAR(255),
		@service VARCHAR(255),
		@salePrice VARCHAR(255),
		@tableDefault2 VARCHAR(100),
		@IsCasePack BIT,
		@defaultBreakCasePack BIT,
		@minQty VARCHAR(10),
		@classificationTable VARCHAR(255)
        
	SELECT 
		@defaultBUYDBESTTemplate = dbo.get_feedmapper_value ('Conf', 'BUYDBESTTemplate', 'DV',@CurrentvendorName),
		@vendorTable = dbo.get_feedmapper_value ('Conf', 'VendorTable', 'DV',@CurrentvendorName),
		@actualvendorTable = PFU.dbo.fn_Get_FeedMapper_Value('Conf', 'VendorTable', 'DV',@CurrentvendorName),
		@inventoryTable = dbo.get_feedmapper_value ('Conf', 'InventoryTable', 'DV',@CurrentvendorName),
		@localImagePath = dbo.get_feedmapper_value ('Conf', 'LocalImagePath', 'DV',@CurrentvendorName),
		@localImageFormat = dbo.get_feedmapper_value ('Conf', 'LocalImageFormat', 'DV',@CurrentvendorName),
		@image = dbo.get_feedmapper_value ('Staging', 'image', 'SF',@CurrentvendorName),
		@minprice = CASE WHEN dbo.get_feedmapper_value ('Conf', 'MinPrice', 'DV',@CurrentvendorName) = '' THEN NULL ELSE dbo.get_feedmapper_value ('Conf', 'MinPrice', 'DV',@CurrentvendorName) END,
		
		
		@weight = dbo.get_feedmapper_value ('Staging', 'weight', 'SF',@CurrentvendorName),
		@actualWeight = 'ActualWeight',
		@courier = dbo.get_feedmapper_value ('Conf', 'Courier', 'DV',@CurrentvendorName),
		@service = dbo.get_feedmapper_value ('Conf', 'Service', 'DV',@CurrentvendorName),
		@salePrice = dbo.get_feedmapper_value ('Staging', 'Sale Price', 'SF',@CurrentvendorName),
		@minQty = dbo.get_feedmapper_value ('Conf', 'MinQuantity', 'DV',@CurrentvendorName),
		@IsCasePack = ISNULL(dbo.get_feedmapper_value('Staging', 'IsCasePack', 'DV',@CurrentvendorName), 'FALSE'),
		@defaultBreakCasePack = ISNULL(dbo.get_feedmapper_value('Staging', 'BreakCasePack', 'DV',@CurrentvendorName), 'FALSE'),
		@classificationTable = dbo.Get_FeedMapper_Value ('Conf', 'ClassificationTableName', 'DV',@CurrentvendorName)

	CREATE TABLE #BUYDBEST_Template (
		BUYDBESTTemplate VARCHAR(100) NOT NULL,
		isDone BIT NOT NULL
	)
	
	IF (@IsCasePack = 'TRUE')
	BEGIN
	
		UPDATE dbo.BUYDBEST_FEED_MAPPER
		SET SourceField = 'MinCasePackPrice'
		WHERE StagingField = 'MinCasePackPrice'
		AND FieldType = 'Staging' and VendorName =@CurrentvendorName 
	
		UPDATE dbo.BUYDBEST_FEED_MAPPER
		SET SourceField = 'MinCasePackWeight'
		WHERE StagingField = 'MinCasePackWeight'
		AND FieldType = 'Staging' and VendorName =@CurrentvendorName 

	END
	
	PRINT'1'

	-- Assuming only one template per vendor, at the moment
	SET @sql = '
			UPDATE PFU.dbo.' + @vendorTable  + '
			SET BUYDBESTTemplate = ''' + @defaultBUYDBESTTemplate + ''''

	EXEC (@sql)
	
	PRINT'2'
	
	INSERT INTO #BUYDBEST_Template (BUYDBESTTemplate, isDone) VALUES (@defaultBUYDBESTTemplate, 0)

    PRINT'3'

	-- Clean-up the staging table to make sure there is no previous data exist for the given vendor
	DELETE FROM  dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC where VendorName =@CurrentvendorName 	
	
	PRINT'4'	

	WHILE EXISTS (SELECT '*' FROM #BUYDBEST_Template WHERE isDone = 0)
	BEGIN
		-- Get one column at a time for processing
		SELECT TOP 1
			@BUYDBESTTemplate = BUYDBESTTemplate
		FROM #BUYDBEST_Template
		WHERE isDone = 0
        
		SET @BUYDBESTTemplateTable = @vendorTable + '_BUYDBEST_' + @BUYDBESTTemplate
		SET @BUYDBESTTemplateStagingTable = @BUYDBESTTemplateTable + '_STAGING'

		-- Create the Vendor Sears Category table, if they do not exist

		EXEC dbo.pr_vendor_BUYDBEST_template_creator @vendorTable, @BUYDBESTTemplate
      
		-- Create a Staging Table for the Vendor Category Table as this will be used for the Update cases.

		IF NOT EXISTS (
			SELECT '*' FROM Sys.Tables WHERE NAME = @BUYDBESTTemplateStagingTable
		)
		BEGIN
			EXEC ('SELECT TOP 0 * INTO ' + @BUYDBESTTemplateStagingTable + ' FROM ' + @BUYDBESTTemplateTable)
			PRINT 'Table ' + @BUYDBESTTemplateStagingTable + ' CREATED'
		END
		


		EXEC ('TRUNCATE TABLE ' + @BUYDBESTTemplateStagingTable)

      
		-- Initialization
		SET @insertList = 'INSERT INTO dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC ('
		SET @selectList = 'SELECT '

		SELECT @columnCount = COUNT(*) 
		FROM dbo.BUYDBEST_FEED_MAPPER
		WHERE FieldType = 'Staging' and VendorName =@CurrentvendorName 

		IF OBJECT_ID ('Tempdb..#Feed_Mapper') <> 0
			DROP TABLE #Feed_Mapper

		-- Load the temp table
		SELECT 
			Sno, 
			FieldType, 
			'[' + sourceField + ']' AS sourceField, 
			'[' + stagingField + ']' AS stagingField, 
			defaultValue, 
			CAST (0 AS BIT) AS isDone
		INTO #Feed_Mapper
		FROM dbo.BUYDBEST_FEED_MAPPER 
		WHERE FieldType = 'Staging' and VendorName =@CurrentvendorName 

		-- Make sure that the columns that does not have mapping goes in as NULL	
		UPDATE #Feed_Mapper
		SET sourceField = NULL
		WHERE sourceField = ''

		-- Loop through for every columns
		WHILE (@columnCount > 0)
		BEGIN
			-- Initialization
			SET @tableDefault = NULL
			SET @BUYDBESTField = NULL

			-- Get one column at a time for processing
			SELECT TOP 1
				@Sno = Sno,
				@sourceField = sourceField,
				@stagingField = stagingField,
				@defaultValue = defaultValue
			FROM #Feed_Mapper
			WHERE isDone = 0

			-- Sears Field is same as Staging Field
			SELECT @BUYDBESTField = REPLACE(REPLACE(@stagingField,'[',''),']','')
           
            
			-- Get the default value defined as part of the table schema, if exists
			SELECT @tableDefault = REPLACE(REPLACE(REPLACE(REPLACE(D.definition, '(', ''), '(', ''), ')', ''), ')', '')
			FROM SYS.OBJECTS A
				INNER JOIN SYS.COLUMNS B ON (A.object_id = B.object_id)
				INNER JOIN SYS.OBJECTS C ON (B.default_object_id = C.object_id)
				INNER JOIN SYS.DEFAULT_CONSTRAINTS D ON (C.object_id = D.object_id)
			WHERE A.name = @BUYDBESTTemplateTable
			AND B.name = @BUYDBESTField
			AND B.default_object_id <> 0
			
           
          
			-- Form the insert column lists
			SET @insertList = @insertList + @stagingField + ','

			  -- Get the source column, if exists and truncate the SourceField for "title" if the length is more than 128
            IF @sourceField IS NOT NULL AND @stagingField = '[name]'
            BEGIN
            
               SET @sourceField = dbo.Get_Together (@sourceField)
                
               SET @sourceField = 'Left(' + @sourceField + ',350)'
               
               SET @selectList = @selectList + @sourceField + ' AS ' + @stagingField + ','
               
               
               
            END 
            
			-- Get the source column, if exists
			ELSE IF @sourceField IS NOT NULL
			BEGIN
				SET @selectList = @selectList + dbo.get_together (@sourceField) + ' AS ' + @stagingField + ','
			END

			-- Get the default value entered in the Feed Mapper Excel, if exists
			ELSE IF @defaultValue IS NOT NULL
			BEGIN
				SET @selectList = @selectList + '''' + @defaultValue + '''' + ' AS ' + @stagingField + ','
			END

			-- Get the default value defined in the table, if exists
			ELSE IF @tableDefault IS NOT NULL
			BEGIN
				SET @selectList = @selectList + @tableDefault + ' AS ' + @stagingField + ','
			END

			-- Just insert NULL, if none of the above conditions satisfied
			ELSE
			BEGIN
				SET @selectList = @selectList + 'NULL' + ' AS ' + @stagingField + ','
			END




			-- Update the temp table to move to the next column
			UPDATE #Feed_Mapper
			SET isDone = 1
			WHERE Sno = @Sno

			SET @columnCount = @columnCount - 1
		END

		DECLARE @filter VARCHAR(500)

		SELECT @filter = ISNULL(@filter + ' AND ', '') + 'ISNUMERIC (' + sourceField + ') = 1 '  FROM #Feed_Mapper
		WHERE stagingField IN ('[qty]', '[weight]', '[price]')
		AND sourceField IS NOT NULL


		SET @insertList = @insertList + ' [BUYDBESTTemplate]) '
		SET @selectList = @selectList + ' [BUYDBESTTemplate] FROM PFU.dbo.' + @vendorTable
		SET @whereClause = ' WHERE [BUYDBESTTemplate] = ''' + @BUYDBESTTemplate + ''' 
							 AND ' + @filter

		SET @sql = @insertList + @selectList + @whereClause
       
		IF @debug = 'Y'
		BEGIN
			PRINT @sql
			RETURN
		END

    print @sql    
         
		-- Execute the insert statement to load the data into the staging table
		EXEC (@sql)
            

		-- Move to the next record in the loop
		UPDATE #BUYDBEST_Template
		SET isDone = 1
		WHERE BUYDBESTTemplate = @BUYDBESTTemplate

	END
	
	PRINT'5'
 
	-- Update the Qty from the Inventory table, if that comes in the feed
	IF @inventoryTable <> ''
	BEGIN
        
		DECLARE 
			@inventoryKeyField VARCHAR(255), 
			@InventoryQtyField VARCHAR(255)

		SET @inventoryKeyField = dbo.get_feedmapper_value('Conf', 'InventoryKeyField', 'SF',@CurrentvendorName )
		SET @InventoryQtyField = dbo.get_feedmapper_value('Conf', 'InventoryQtyField', 'SF',@CurrentvendorName )

        
		SET @sql = '
				UPDATE stg
				SET stg.' + @InventoryQtyField + ' = inv.[' + dbo.get_feedmapper_value('Inventory', @InventoryQtyField, 'SF',@CurrentvendorName ) + ']
				FROM dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC stg
					LEFT OUTER JOIN BUYDBEST.dbo.[' + @inventoryTable + '] inv ON (
						stg.[' + @inventoryKeyField + '] = inv.[' + dbo.get_feedmapper_value('Inventory', @inventoryKeyField, 'SF',@CurrentvendorName ) + ']
					) WHERE VendorName ='''+@CurrentvendorName+''''
		
        EXEC (@sql)
		
	END
	
	PRINT'6'
    
	-- Update the Quantity from stock status if it is mapped.
	IF (dbo.get_feedmapper_value ('Staging', 'StockStatus', 'SF',@CurrentvendorName ) <> '' 
		OR dbo.get_feedmapper_value ('Conf', 'InventoryQtyField', 'SF',@CurrentvendorName ) = 'StockStatus'
	)
	BEGIN

		-- This section is to clean-up the Stock Status field, when it does not have the statard allowed t`
		UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
		SET qty = CASE 
							WHEN StockStatus = 'in stock' THEN 50
							WHEN StockStatus = 'not in stock' THEN 0
							WHEN ISNUMERIC(StockStatus) = 1 AND CAST(StockStatus AS INT) > 10 THEN CAST(StockStatus AS INT) - 10
							ELSE NULL
						END
						WHERE VendorName =@CurrentvendorName 
	END
	
	PRINT'7'
	
	IF @localImagePath <> '' AND @image = ''
	BEGIN
		UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
		SET [image] = @localImagePath + @localImageFormat Where VendorName =@CurrentvendorName 
	END
	
	PRINT'8'
     
  --TO update Shipping Length/Width/Height as 1*1*1
    
  
	UPDATE VENDOR_BUYDBEST_TEMPLATE_GENERIC SET
		    [weight] =1 where [weight] is null or cast([weight] as numeric(8,2)) <=0	  AND VendorName =@CurrentvendorName  	    
    PRINT'9' 
     -- To remove the $ symbol from the ItemPrice field as it is causing conversion error eventhough pass through ISNUMERIC check
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
	SET [price] = REPLACE([price], '$', '')
	WHERE [price] LIKE '%$%' AND VendorName =@CurrentvendorName 
	
	PRINT'10'
	-- To remove the $ symbol from the MAP field as it is causing conversion error eventhough pass through ISNUMERIC check
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
	SET [special_price] = REPLACE([special_price], '$', '')
	WHERE [special_price] LIKE '%$%' AND VendorName =@CurrentvendorName 
	
	PRINT'11'
	/* 18-May-2010 Change Start */
	DELETE FROM dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
	WHERE CAST([price] AS NUMERIC(18,2)) <= @minprice AND VendorName =@CurrentvendorName 
	/* 18-May-2010 Change End */
	
	PRINT'12'
		  
	--To delete the row from the VENDOR_SEARS_TEMPLATE_GENERIC Table, if the Product Image URL, does not come from the feed  
   -- DELETE FROM dbo.VENDOR_SEARS_TEMPLATE_GENERIC
	--WHERE [Product Image URL] IS NULL	
	
	
	 
	 UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC SET IsCasePack = 'FALSE' WHERE IsCasePack IS NULL AND VendorName =@CurrentvendorName 
	 
	 PRINT'13'
	 
	 IF @IsCasePack = 'TRUE' AND @defaultBreakCasePack = 'FALSE'
	BEGIN
		UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC SET name  = name  + ' - Case of ' + CONVERT(VARCHAR(4), NumberOfPieces)
			WHERE NumberOfPieces IS NOT NULL and NumberOfPieces > 1 AND VendorName =@CurrentvendorName 
	END
	
	PRINT'14'
			
    IF (@minQty <> '')
	BEGIN
        UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC
		
		SET qty = CASE WHEN CAST(@minQty AS INT) > CAST(qty AS INT) THEN 0 ELSE qty - CAST(@minQty AS INT) END
		WHERE VendorName =@CurrentvendorName 
	END 	
	
	PRINT'15'
	print @classificationTable
	IF ((@classificationTable <> '' )OR( @classificationTable IS NOT NULL))
	BEGIN
		SET @sql = '
				UPDATE gen
				SET gen.TemplateCategory = cat.BUYDBESTTemplateCategory
				FROM dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC gen
					INNER JOIN dbo.' + @classificationTable + ' cat ON (
						gen.[MultipleCategories] = cat.Category) WHERE VendorName ='''+@CurrentvendorName +'''
						
					
				'
		print @sql		
		EXEC (@sql)
	END	
	
	PRINT'16'
	DELETE FROM dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC WHERE ([image] NOT LIKE '%.jpeg')  
													AND ([image] NOT LIKE '%.jpg' )
													AND ([image] NOT LIKE '%.gif' )
													AND ([image] NOT LIKE  '%.tiff') 
													AND ([image] NOT LIKE  '%.png')
													AND VendorName =@CurrentvendorName 
													
	PRINT'17'

	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]=REPLACE([name],'&nbsp','')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]=REPLACE([name],'&#039','''')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]=REPLACE([name],'&#39','''')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]=REPLACE([name],'&quot','"')  WHERE VendorName =@CurrentvendorName 


	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]=REPLACE([Description],'&nbsp','')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]=REPLACE([Description],'&#039','''')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]=REPLACE([Description],'&#39','''')  WHERE VendorName =@CurrentvendorName 
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]=REPLACE([Description],'&quot','"')  WHERE VendorName =@CurrentvendorName 


	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]= [dbo].[fn_Replace_AccentCharacters]([Description]) WHERE VendorName =@CurrentvendorName 

	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]= [dbo].[fn_Replace_AccentCharacters]([name]) WHERE VendorName =@CurrentvendorName 


	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]= [dbo].[RemoveSpecialChars]([Description]) WHERE VendorName =@CurrentvendorName 

	
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name] = [dbo].[RemoveSpecialChars]([name] ) WHERE VendorName =@CurrentvendorName 


	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [Description]= [dbo].[fnRemoveMultipleSpaces]([Description]) WHERE VendorName =@CurrentvendorName 
	
	
	
	UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [name]  = [dbo].[fnRemoveMultipleSpaces]([name] ) WHERE VendorName =@CurrentvendorName 

    UPDATE dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC  SET [is_in_stock]=CASE WHEN qty>0 THEN '1' else '0' end

	delete from dbo.VENDOR_BUYDBEST_TEMPLATE_GENERIC where VendorName = @CurrentvendorName and ([name] is  null or [name] ='')

PRINT'18'
--COMMIT TRAN @CurrentvendorName

END TRY
BEGIN Catch
 IF (XACT_STATE()) = -1
    BEGIN
        PRINT
            N'The transaction is in an uncommittable state. ' +
            'Rolling back transaction.'
        --ROLLBACK TRAN @CurrentvendorName;
    END;
INSERT INTO ErrorLog VALUES( @CurrentvendorName, DB_NAME() + '.' + OBJECT_SCHEMA_NAME(@@PROCID) + '.' + OBJECT_NAME(@@PROCID),Error_Number(),ERROR_MESSAGE())

Delete from  dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_generic_load =@CurrentvendorName       
   
PRINT'19'

ENd Catch
 
Delete from  dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_generic_load=@CurrentvendorName 

End


ELSE 
Begin
Waitfor delay '00:00:10'
print 'Waiting'
Goto BeginBUYDBESTSproc;
end 

end	





