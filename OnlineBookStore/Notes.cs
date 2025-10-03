
// ================================== 01: Identity Scaffolding ==============================
/*
    . When project is built first we will not see Identity pages like Login and Register to see them we need to scaffold them.
    . Since we have configured the Individual Identity Account so it has configured alot of things for us e.g. DBContext class, Migrations folder etc
    . We dont have to configure DBContext cuz it has did connectivity stuff by which we connet to DB.
    . When u run the project u will see Identity/Account/Login when click on Login link but when u see the folder structure u will not see any Identity folder for this we have to do scafolding.
    . Scaffolding is a way to quickly generate code for common application features, such as user authentication, CRUD operations, and more.
    . In prod we dont have the this confirmation pages cuz we usually configure our email provider with this project so when we click on confirm email the user will get the email and when he click on the link in email he will be redirected to our app and his email will be confirmed.
    . Since we are developing the project so we will not config email service for this

*/


// ====================================== 02: Entities, Migrations ==================================
/*
    . We have passed GenreId in Book table to create the relationship between Book and Genre.
    . One Book can have only one Genre but one Genre can have many Books.
    . So this is one to many relationship.
    . We have to create the navigation properties in both the classes to create the relationship.

*/


// ==================================== 03: Seeding admin record in the database ==================================
/*
    Object: Seed default admin user and complete authentication flow
    . Adding role management
    . We have made enum of roles now we have to seed these roles in the database cuz its empty now.
    . We have to create a class to seed the roles in the database.
    . We have to call this class in the Program.cs file.
    . We have to create a admin user and assign the admin role to that user.
    . Suppose you run a bookstore:
        When you need a cashier, you don’t go and hire one yourself every time.
        Instead, the store manager (your system) already provides you a cashier when you ask.
        That’s what DI does: it hands you the service you need, instead of you creating it manually.
    . When your app starts (Program.cs / Startup.cs), .NET registers these managers for you.
        Then later, you just say:
            var userMgr = service.GetService<UserManager<IdentityUser>>();
        and DI gives you a ready-to-use UserManager.
    . I used UserManager and RoleManager (given by dependency injection) to create default roles and an admin user in the database.
    
    1. Used built-in .NET services
        UserManager<IdentityUser> and RoleManager<IdentityRole> are services provided by ASP.NET Identity.
        These know how to create users, assign roles, etc. You didn’t write them — .NET gives them to you.

    2. Made your own service-like helper (DbSeeder)
        You wrote DbSeeder.SeedDefaultData(...).
        But notice: it does not get “given to users of the app”.
        Instead, it’s just a helper method you run at startup to prepare default data (roles + admin).

    So the correct understanding is:
        You used .NET’s Identity services (UserManager, RoleManager).
        You wrote a DbSeeder class that calls those services to insert roles and the admin user.
        You are not “giving this service to the user” — it’s just a background startup job to set up the database.

*/






