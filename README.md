# Excel-SQLizer
A simple GUI app to convert Excel data into SQL insert statements.

## Installation
Right now you need to build the project in VisualStudio and use the .exe from the debug folder. It's rough right now.

Sorry about that.

## Using
Excel-SQLizer expects a very specifically structured Excel file in order to convert it into SQL statements (currently, only insert).

The name of the worksheet is the table name. The first row are the column names. All other rows are the data. Soon the first row MUST be the Primary Key, but for now order doesn't matter at all.
The workbook can have as many sheets as desired as long as they follow this structure. 
Each sheet will generate it's own SQL script, which is placed in the same folder as the source Excel file.


## TODOs
* Create some kind of installer or at least provide a zip of DLLs and .exe. 
* Make the UI less offensive to the human eye.
* Add support for different statements (UPDATE and DELETE coming soon)
* Add option for selecting an output destination instead of always generating the SQL scripts in the same folder as Excel source file.
  * But keep the option for that behaviour.
