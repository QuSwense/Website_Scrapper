The database methods that are needed for website scrapping:
1. Create database
	a. The name of the database is by the name of the application topic used for web scrapping
	b. The path of the database is - network path / local path
	c. Define the type of database
2. Create tables as defined in tables_metadata.csv
	a. Create metadata table with columns 'tblmdt':
		+ table name - The list of tables used in storing webscrapped data (do not include metadta tables)
		+ display name - The name used for display in a UI application
		+ description - The description used to display in UI application
		+ url - The reference website url
	b. Create metdata table with columns, naming convention '<table_name>mdt' tables as defined in tables_columns.csv
		+ column name - The column name of the table
		+ display name - The name used for display in a UI application
		+ description - The description used to display in UI application
		+ url - The reference website url
	c. Create metadata table with rows, naming convention '<table_name>_<col_name>mdt' tables as defined in tables_columns.csv
		+ row id - Each table must have a unique identifier
		+ display name - The name used for display in a UI application
		+ description - The description used to display in UI application
		+ url - The reference website url
		+ xpath - The relative XPath in the html page
	d. Create data tables as defined in tables_columns.csv
3. Read <app>Scrap.xml config file
	a. Read the 'Scrap' tag with url and xpath. Initialize the HtmlNode element using the url and Xpath
		+ If url is present load HtmlDocument and get the parent root node
		+ If XPath is present then navigate to the node (By default use multiple node)
		+ If name attribute is present then save it (it will be used to load the table)
	b. Read the child elements
	c. If the child element is 'Scrap' goto step <a>
	d. If child element is 'Columns' then get the list of its child elements 'Column'
	e. Load the table ('name' attribute in step 3.<a>) into memory
	f. From the list of 'Column' get the columns with 'ispk' true
		+ If no such columns is found then by default always insert a new row
		+ If columns are found, create a temporary dictionary from the loaded table by the column values for quick reference
	e. Now loop through the Scrap XPath node saved in Step 3.<a>
	f. For each node in Step 3.<e>, loop through each Column in Step 3.<d>
	g. For each column, get the name and xpath
		+ If XPath not present throw error
		+ If name not present throw error
		+ Get the single node from xpath
		+ Get the value
		+ If manipulate tag is present, then manipulate data before returning data to the dataset list
	h. Once all column data is loaded in memory, then save the row