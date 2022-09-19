/*CREACION BASE DE DATOS*/
create database DBTIENDA

GO

USE DBTIENDA

GO

/*TABLAS*/
/*USUARIO*/
CREATE TABLE Usuario(
IdUsuario int primary key identity,
Nombre varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Contrasena varchar(150),
Reestablacer bit default 1,
Rol varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
go

/*CATEGORIA*/
CREATE TABLE Categoria (
IdCategoria int primary key identity,
NombreCategoria varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
go
/*MARCA*/
CREATE TABLE Marca (
IdMarca int primary key identity,
NombreMarca varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
go

/*PRODUCTO*/
CREATE TABLE Producto (
IdProducto int primary key identity,
Nombre varchar(500),
DescripcionProducto varchar(500),
IdMarca int references Marca(IdMarca),
IdCategoria int references Categoria(IdCategoria),
Precio decimal(10,2) default 0,
Stock int,
RutaImagen varchar(100),
NombreImagen  varchar(100),
Activo bit default 1,
FechaRegistro datetime default getdate()
)
go

CREATE TABLE DetalleVenta (
IdDetalleVenta int primary key identity,
IdVenta int references Venta(IdVenta),
IdProducto int references PRODUCTO(IdProducto),
Cantidad int,
Total decimal(10,2),
IdTransaccion varchar(50)
)
go

CREATE TABLE Venta (
IdVenta int primary key identity,
IdCliente int references CLIENTE(IdCliente),
TotalProducto int,
MontoTotal decimal(10,2),
Contacto varchar(50),
CodigoPostal varchar(100),
Telefono varchar(50),
Direccion varchar(500),
IdTransaccion varchar(50),
FechaVenta datetime default getdate()
)
go

CREATE TABLE Cliente (
IdCliente int primary key identity,
Nombre varchar(100),
Apellidos varchar(100),
Correo varchar(100),
Contrasena varchar(150),
Reestablecer bit default 0,
FechaRegistro datetime default getdate()
)
go

/*Procedimientos Almacenados
         USUARIO*/
create procedure sp_GuardarUsuario
(
@Nombre varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Rol varchar(100),
@Activo bit,
@Contrasena varchar (100),
@Mensaje varchar(500) output
)
as
begin
	IF  NOT EXISTS (SELECT * FROM USUARIO WHERE Correo = @Correo)
	begin
	insert into USUARIO(Nombre,Apellidos,Correo,Rol, Activo, Contrasena) values
	(@Nombre,@Apellidos,@Correo, @Rol,@Activo,@Contrasena)
end
else
		set @Mensaje ='El correo del usuario ya existe'
		end



create procedure sp_EditarUsuario
(
@IdUsuario int,
@Nombre varchar(100),
@Apellidos varchar(100),
@Correo varchar(100),
@Rol varchar(100),
@Activo bit
)
as
begin
	update USUARIO set 
	Nombre = @Nombre,
	Apellidos = @Apellidos,
	Correo = @Correo,
	Rol = @Rol,
	Activo = @Activo
	where IdUsuario = @IdUsuario
	
end

go	


/*CATEGORIA*/
create procedure sp_GuardarCategoria
(
@NombreCategoria varchar (100),
@Activo bit,
@Mensaje varchar(500) output
)
as
begin
	IF  NOT EXISTS (SELECT * FROM Categoria WHERE NombreCategoria = @NombreCategoria)
	begin
	insert into Categoria(NombreCategoria, Activo) values
	(@NombreCategoria,@Activo)
	end
	else
	set @Mensaje ='La categoria ya existe'
	end
go

create procedure sp_EditarCategoria
(
@IdCategoria int,
@NombreCategoria varchar(100),
@Activo bit,
@Mensaje varchar (500) output
)
as
begin
	update Categoria set 
	NombreCategoria = @NombreCategoria,
	Activo = @Activo
	where IdCategoria = @IdCategoria
end

go	

create procedure sp_EliminarCategoria
(
@IdCategoria int,
@Mensaje varchar(500) output
)
as
begin
	IF NOT EXISTS (select *from Producto p
	inner join Categoria c on c.IdCategoria = p.IdCategoria
	where p.IdCategoria = @IdCategoria)
	begin 
		delete top (1) from Categoria where IdCategoria = @IdCategoria
	end
	else
	set @Mensaje = 'La categoría esta relacionada con un producto'
	end
go

/*MARCA*/
create procedure sp_GuardarMarca
(
@NombreMarca varchar (100),
@Activo bit,
@Mensaje varchar(500) output
)
as
begin
	IF  NOT EXISTS (SELECT * FROM Marca WHERE NombreMarca = @NombreMarca)
	begin
	insert into Marca(NombreMarca, Activo) values
	(@NombreMarca,@Activo)
	end
	else
	set @Mensaje ='La Marca ya existe'
	end
go

create procedure sp_EditarMarca
(
@IdMarca int,
@NombreMarca varchar(100),
@Activo bit,
@Mensaje varchar (500) output
)
as
begin
	update Marca set 
	NombreMarca = @NombreMarca,
	Activo = @Activo
	where IdMarca = @IdMarca
end

go

create procedure sp_EliminarMarca
(
@IdMarca int,
@Mensaje varchar(500) output
)
as
begin
	IF NOT EXISTS (select *from Producto p
	inner join Marca c on c.IdMarca = p.IdMarca
	where p.IdMarca = @IdMarca)
	begin 
		delete top (1) from Marca where IdMarca = @IdMarca
	end
	else
	set @Mensaje = 'La Marca esta relacionada con un producto'
	end
go

/*Producto*/
create procedure sp_GuardarProducto
(
@Nombre varchar(100),
@DescripcionProducto varchar(100),
@IdMarca int,
@IdCategoria int,
@Precio decimal(10,2),
@Stock int,
@Activo bit,
@Mensaje varchar (500) output 
)
as
begin
	IF NOT EXISTS (select * from Producto where Nombre = @Nombre)
	begin
	insert into Producto(Nombre, DescripcionProducto, IdMarca, IdCategoria, Precio, Stock, Activo) values
	(@Nombre, @DescripcionProducto,@IdMarca, @IdCategoria, @Precio,@Stock, @Activo)
	end
	else
	set @Mensaje ='El producto ya existe'
end
go

create procedure sp_EditarProducto
(
@IdProducto int,
@Nombre varchar(100),
@DescripcionProducto varchar(100),
@IdMarca int,
@IdCategoria int,
@Precio decimal(10,2),
@Stock int,
@Activo bit,
@Mensaje varchar (500) output 
)
as
begin
	
	update Producto set 
	Nombre = @Nombre,
	DescripcionProducto = @DescripcionProducto, 
	IdMarca = @IdMarca, 
	IdCategoria = @IdCategoria, 
	Precio = @Precio, 
	Stock = @Stock, 
	Activo = @Activo
	where IdProducto = @IdProducto
	
end
go

create procedure sp_EliminarProducto
(
@IdProducto int,
@Mensaje varchar(500) output
)
as
begin
	IF NOT EXISTS (select * from DetalleVenta dv
	inner join Producto p on p.IdProducto = dv.IdProducto
	where p.IdProducto = @IdProducto)
	begin
	delete top(1) from Producto where IdProducto = @IdProducto
	end
	else
	set @Mensaje = 'El producto se encuentra relacionado a una venta'
end
go

/*Reporte Dashboard*/
create proc sp_ReporteDashboard
as
begin

select

(select count(*) from Cliente) [TotalCliente],
(select ISNULL(sum(Cantidad),0) from DetalleVenta)[TotalVenta],
(select count(*) from Producto) [TotalProducto]
end
go 

/*Reporte VENTAS*/
create proc sp_ReporteVentas(
@fechainicio varchar(10),
@fechafin varchar(10),
@idtransaccion varchar(50)
)
as
begin
	set dateformat dmy;

select CONVERT(char(10),v.FechaVenta,103)[FechaVenta], CONCAT(c.Nombre,' ',c.Apellidos)[Cliente],
p.Nombre[Producto], p.Precio, dv.Cantidad, dv.Total, v.IdTransaccion
from DetalleVenta dv
inner join Producto p on p.IdProducto = dv.IdProducto
inner join Venta v on v.IdVenta = dv.IdVenta
inner join Cliente c on c.IdCliente = v.IdCliente
where CONVERT(date, v.FechaVenta) between @fechainicio and @fechafin

and v.IdTransaccion = iif (@idtransaccion = '', v.IdTransaccion,@idtransaccion)

end
go