If you need Compress and Decompress in earlier versions of SQL Server than 2016.

Since SQLCLR doesn't support overloading for a function, you will have to convert the value to VARBINARY(MAX) when calling the Compress and Decompress functions.
