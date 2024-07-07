# To-do List Application

## Software Architecture

The solution have a [3-tier architecture](https://en.wikipedia.org/wiki/Multitier_architecture):
  * The presentation tier — the web application named *TodoListApp.WebApp* that provides [browser user interface](https://en.wikipedia.org/wiki/Browser_user_interface) for the end-users allowing them to manage their to-do lists.
  * The logic (application) tier — the web API application named *TodoListApp.WebApi* that provides a RESTful API the web application must use to retrieve or save to-do lists or user's data.
  * The data tier is the relational database management system for storing to-do lists and other user's data.

![Architecture](images/architecture.png)

The application store its data in two relational databases.
  * The *UsersDb* database used to store Identity configuration data (user names, passwords, and profile data).
  * The *TodoListDb* database used to store the user data (to-do lists, tasks, tags and comments).
