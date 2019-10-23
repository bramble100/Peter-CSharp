# Peter
Solution for the stock exchange related side project.

Main functions:
* Provides access to datavendor's pages, namely:
  * downloads pages
  * parses the market data info from HTML tables
  * updates the market data
* Provides access to maintain registry entries (basic stock data)
* Creates analyses based on:
  * latest market data info
  * registry entries
  * screening options
  

Coding guidelines:
* [Approved Verbs for PowerShell Commands | Microsoft Docs](https://docs.microsoft.com/en-us/powershell/scripting/developer/cmdlet/approved-verbs-for-windows-powershell-commands)

Important dependencies:
* [Autofac: Inversion of Control container](https://autofac.org/)
* [HtmlAgilityPack: HTML parser](https://html-agility-pack.net/)
* [Moq: Mocking framework](https://github.com/moq/moq4)
* [Newtonsoft.Json: JSON framework](https://www.newtonsoft.com/json)
* [NLog: logging platform](https://nlog-project.org/)
* [NUnit: Unit-testing framework](https://nunit.org/)