
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