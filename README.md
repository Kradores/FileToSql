# FileToSql

### Experiments to find the fastest way to insert data from an excel file to sql.</br>
There are 4 different endpoints that insert a file:</br>
<code>/upload/bulk/partsmaster</code> is for a smaller file, with a simple add + save changes.</br>
<code>/upload/bulk/listprice</code> same as previous one, but with a larger file - time to insert ~90 sec.</br>
<code>/upload/bulk/parallel/listprice</code> this one uses paralelism and each task has its own db context - time to insert ~80 sec.</br>
<code>/upload/bulk/ext/listprice</code> uses <code>BulkInsertAsync</code> from <code>Z.EntityFramework.Extensions.EFCore</code> - time to insert ~16 sec.</br>
