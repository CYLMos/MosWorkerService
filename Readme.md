# Introduction

### This worker service can fetch the stock data from TWSE everyday.
### Some attributes can be modified in appsettings.json.

# Parameters

### StockNumber
A string array of stock numbers.
For example: ["0050", "0056"]

### FetchUrl
The data will be fetched from this url.
Shouldn't be modified.

### FetchTime
Determine the time to fetch the data.
For example: "16:00:00" means the worker service will fetch data in 16:00:00 everyday.

### CsvPath
The data will be stored to an csv file.
This attrribute will define the location of the file.