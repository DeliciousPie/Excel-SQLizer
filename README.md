# Excel-SQLizer
A simple GUI app to convert Excel data into SQL insert statements.

## Installation
Download the latest release. Application is run from the 'Excel-SQLizer.WFP.exe' file.

## Using
Excel-SQLizer expects a very specifically structured Excel file in order to convert it into SQL statements (update, insert, and delete).

The name of the worksheet is the table name. The first row are the column names. All other rows are the data. First row must be primary key in order for the update and delete statements to work.
The workbook can have as many sheets as desired as long as they follow this structure. 
Each sheet will generate it's own SQL script, which is placed in the same folder as the source Excel file.
