# [Locs 4 Youth](http://abdullahsari.ikdoeict.net/)
An ASP.NET MVC 5 website created for the course ".NET Web Solutions" at Odisee. http://abdullahsari.ikdoeict.net/

## Live Website

<http://abdullahsari.ikdoeict.net/>

## Setup

### Database

You will have to create a new MSSQL database and import the tables provided with the db_dump.sql file.
Do not forget to add the connection string in Web.config:

```xml
<connectionStrings>
<add name="jeugdlocatiebookingConnectionString" connectionString="CHANGE TO YOUR DATABASE" providerName="System.Data.SqlClient" />
</connectionStrings>
```
### E-mail

The application is using SMTP e-mail services to send a few e-mails when a particular action happens.
Adjust the Web.config according to your credentials:

```xml
<system.net>
<mailSettings>
    <smtp deliveryMethod="Network" from="YOUR EMAIL">
    <network host="smtp.gmail.com" port="587" userName="YOUR EMAIL" password="YOUR PASSWORD" enableSsl="true" />
    </smtp>
</mailSettings>
</system.net>
```

## License

[MIT](https://opensource.org/licenses/MIT)