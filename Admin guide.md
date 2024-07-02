The admin is a special kind of user that can add delete or edit books, he can also do everything a normal user can.
## Create/Edit/Delete book menu
Here you can create/edit or delete a book.
It can only be seen by admin user.

![[Pasted image 20240701225440.png]]
![[Pasted image 20240701225324.png]]
![[Pasted image 20240701225457.png]]

# Database overview

### Book entity:
Id - The primary key 
Title  - The title of the Book
Author  - The author of the Book
Genre - The genre of the Book
PublicationDate - The object in which we store the book release date  
ImageData - The object in which we store images, they are stored as a byte array
  
Many to many relationship with Cart object 

### Cart entity
UserId  - The primary key 
User   - The Currently logged in user  
Amount  - The ammount for the book  
BookId  - The Book that the user has in his cart One to many  



### User entity
UserId  - The primary key
Email  - The User login email  
Name    - The User Name  
Surname  - The User Surname  
Role   - The role for the user(e. g. Admin, user)  
Password  - The password of the user  

Many to many relationship with Book object