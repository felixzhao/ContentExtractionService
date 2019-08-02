# ContentExtractionService
This project aims to create a RESTFUL API service to extract content from plain text input.

There are several techs are used in this project:
1. DotNet Core Web API
2. Log trace by Serilog
3. Mapping business object to contracts by AutoMapper
4. Unit Tests by XUnit

Business Logic of this project:
Import data from the text received via email.
The data will either be:
- Embedded as ‘islands’ of XML-like content
- Marked up using XML style opening and closing tags
- Opening tags that have no corresponding closing tag. In this case, the whole message should be rejected.
- Missing <total>. In this case, the whole message should be rejected.
- Missing <cost_centre>. In this case, the ‘cost centre’ field in the output should be defaulted to ‘UNKNOWN’.

Development Environment:
- Visual Studio 2019 for mac (Community) Version 8.2
- macOS Sierra Version 10.12.6

GST Calculate Results in Unit Tests based on gst online calculator:
https://kiwitax.co.nz/gst-calculator/
