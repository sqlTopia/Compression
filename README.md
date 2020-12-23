If you need Compress and Decompress in earlier versions of SQL Server than 2016.

Since SQLCLR doesn't support multiple interfaces for a function, you will have to convert the value to VARBINARY(MAX) when calling the Compress and Decompress functions.
