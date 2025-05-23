USE [BUYDBEST]
GO
/****** Object:  StoredProcedure [dbo].[pr_vendor_BUYDBEST_template_update]    Script Date: 09/24/2012 00:18:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/****** Object:  StoredProcedure [dbo].[pr_vendor_BUYDBEST_template_update]  ***********/

ALTER PROCEDURE [dbo].[pr_vendor_BUYDBEST_template_update] 
@BUYDBESTTemplateList VARCHAR(255),@vendorName VARCHAR(50)
AS
BEGIN

	SET NOCOUNT ON
	Declare @vendorCount int
	BeginBUYDBESTTUSproc:
	
	
	Set @vendorCount =(Select COUNT(*)  from dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_update is not null)
	If (@vendorCount=0)
	Begin 
	 INSERT INTO dbo.CurrentRunningVendor with(tablock)(pr_vendor_BUYDBEST_template_update) VALUES(@vendorName )
	BEGIN TRY 
	BEGIN TRAN @vendorName	
	DECLARE 
		@update_sql VARCHAR(MAX),		
		@delete_sql VARCHAR(MAX),
		@dup_update_sql VARCHAR(MAX),
		@itempriceupdate_sql VARCHAR(MAX),	
		@BUYDBESTTemplateTable VARCHAR(255),
		@BUYDBESTTemplateStagingTable VARCHAR(255),
		@vendorTable VARCHAR(255),
		@isRepriced BIT,
		@SKUPrefix VARCHAR(10),
		@sql VARCHAR(MAX)

	SELECT @vendorTable = dbo.get_feedmapper_value ('Conf', 'VendorTable', 'DV',@vendorName)
	SELECT @isRepriced = dbo.get_feedmapper_value ('Staging', 'isRepriced', 'DV',@vendorName)
	SELECT @SKUPrefix = dbo.get_feedmapper_value ('Staging', 'SKUPrefix', 'DV',@vendorName)



	IF @BUYDBESTTemplateList LIKE '%ALL%'
	BEGIN

		SET @BUYDBESTTemplateTable = @vendorTable + '_BUYDBEST_ALL'
		SET @BUYDBESTTemplateStagingTable = @BUYDBESTTemplateTable + '_STAGING'

		-- Quantity Reset
		
		EXEC ('UPDATE dbo.' + @BUYDBESTTemplateTable + ' SET qty = 0')

		SET @update_sql = '

		UPDATE target
		SET
		    [store]=stage.[store],
    [websites]=stage.[websites],
    [attribute_set]=stage.[attribute_set],
    [type]=stage.[type],
    [category_ids]=stage.[category_ids],
    [sku]=stage.[sku],
    [name]=stage.[name],
    [meta_title]=stage.[meta_title],
    [meta_description]=stage.[meta_description],
    [image]=stage.[image],
    [small_image]=stage.[small_image],
    [thumbnail]=stage.[thumbnail],
    [availablity_status]=stage.[availablity_status],
    [manufactured_by]=stage.[manufactured_by],
    [price] = CASE WHEN ' + CAST(@isRepriced AS CHAR(1)) + ' = 1 THEN target.[price] ELSE stage.[price] END,
    [special_price]=stage.[special_price],
    [weight]=stage.[weight],
    [status]=stage.[status],
    [visibility]=stage.[visibility],
    [enable_googlecheckout]=stage.[enable_googlecheckout],
    [tax_class_id]=stage.[tax_class_id],
    [description]=stage.[description],
    [short_description]=stage.[short_description],
    [meta_keyword]=stage.[meta_keyword],
    [qty]=stage.[qty],
    [is_in_stock]=stage.[is_in_stock]
	
			
		FROM dbo.' + @BUYDBESTTemplateTable + ' target
			INNER JOIN dbo.' + @BUYDBESTTemplateStagingTable + ' stage ON (target.[sku] = stage.[sku])'

		SET @dup_update_sql = '
			UPDATE target
			SET target.qty = stage.qty
			FROM dbo.' + @BUYDBESTTemplateTable + ' target 
				INNER JOIN dbo.' + @BUYDBESTTemplateStagingTable + ' stage ON (
					''' + @SKUPrefix + ''' + SUBSTRING(target.[SKU], ' + CAST (LEN(@SKUPrefix) + 4 AS VARCHAR) + ', LEN(target.[SKU])) = stage.[SKU]
				)
			WHERE target.[SKU] LIKE ''' + @SKUPrefix + '[_][0-9][0-9][_]%'''
        
        SET @itempriceupdate_sql = '
			UPDATE target
			SET [price] = pricetbl.[price]
			FROM dbo.' + @BUYDBESTTemplateTable + ' target
				INNER JOIN dbo.BUYDBEST_CUSTOM pricetbl ON (target.[sku] = pricetbl.[sku])' 			
         
		SET @delete_sql = '

		DELETE stage
		FROM dbo.' + @BUYDBESTTemplateTable + ' target
			INNER JOIN dbo.' + @BUYDBESTTemplateStagingTable + ' stage ON (target.[sku] = stage.[sku])'

		EXEC (@update_sql)
		EXEC (@dup_update_sql)
		
			IF @isRepriced = 0
			EXEC (@itempriceupdate_sql)
			
		EXEC (@delete_sql)

		-- Zero quantity items need not be INSERTED into BUYDBEST table. That means that if an item comes into the system for the first time
		-- it should have a non-zero qty

		SET @delete_sql = '

		DELETE stage
		FROM dbo.' + @BUYDBESTTemplateStagingTable + ' stage
		WHERE qty = 0'

		EXEC (@delete_sql)
	END
	


COMMIT TRAN @vendorName

END TRY
BEGIN Catch
 IF (XACT_STATE()) = -1
    BEGIN
        PRINT
            N'The transaction is in an uncommittable state. ' +
            'Rolling back transaction.'
        ROLLBACK TRAN @vendorName;
    END;
INSERT INTO ErrorLog VALUES( @vendorName, DB_NAME() + '.' + OBJECT_SCHEMA_NAME(@@PROCID) + '.' + OBJECT_NAME(@@PROCID),Error_Number(),ERROR_MESSAGE())

Delete from  dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_update=@vendorName       
   


ENd Catch
	
Delete from  dbo.CurrentRunningVendor where pr_vendor_BUYDBEST_template_update=@vendorName 

End


ELSE 
Begin
Waitfor delay '00:00:10'
print 'Waiting'
Goto BeginBUYDBESTTUSproc;
end 

end




