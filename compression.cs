using Microsoft.SqlServer.Server;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;

public partial class Database
{
        [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
        public static SqlBytes Compress(SqlBytes data)
        {
                if (data.IsNull)
                {
                        return SqlBytes.Null;
                }

                // Convert to BLOB
                byte[] blob = data.Buffer;

                // Preparing for compression
                MemoryStream compressedData = new MemoryStream();
                GZipStream compressor = new GZipStream(compressedData, CompressionMode.Compress, true);

                // Writting uncompressed data using a DeflateStream compressor
                compressor.Write(blob, 0, blob.Length);

                // Closing compressor to allow ALL compressed bytes to be written
                compressor.Flush();
                compressor.Close();

                // Returning compressed blob
                return new SqlBytes(compressedData);
        }
        [SqlFunction(IsDeterministic = true, DataAccess = DataAccessKind.None)]
        public static SqlBytes Decompress(SqlBytes blob)
        {
                if (blob.IsNull)
                {
                        return SqlBytes.Null;
                }

                // Preparing to read data from compressed stream
                GZipStream decompressor = new GZipStream(blob.Stream, CompressionMode.Decompress, true);

                // Initializing variables
                int bytesRead;
                int chunkSize = 8192;
                byte[] chunk = new byte[chunkSize];

                // Preparing destination stream to hold decompressed data
                MemoryStream decompressedData = new MemoryStream();

                try
                {
                        // Reading from compressed stream 
                        while ((bytesRead = decompressor.Read(chunk, 0, chunkSize)) > 0)
                        {
                                // Writting decompressed data
                                decompressedData.Write(chunk, 0, bytesRead);
                        }
                }
                finally
                {
                        // Cleaning up
                        decompressor.Close();
                }

                // Returning a decompressed BLOB
                return new SqlBytes(decompressedData);
        }
};
