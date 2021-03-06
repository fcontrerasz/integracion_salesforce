USE [master]
GO
/****** Object:  Database [ws_salesforce]    Script Date: 11/09/2018 23:40:15 ******/
CREATE DATABASE [ws_salesforce] ON  PRIMARY 
( NAME = N'ws_salesforce', FILENAME = N'F:\DATA\ws_salesforce.mdf' , SIZE = 209920KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ws_salesforce_log', FILENAME = N'F:\DATA\ws_salesforce.ldf' , SIZE = 60736000KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ws_salesforce] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ws_salesforce].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ws_salesforce] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ws_salesforce] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ws_salesforce] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ws_salesforce] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ws_salesforce] SET ARITHABORT OFF 
GO
ALTER DATABASE [ws_salesforce] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ws_salesforce] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [ws_salesforce] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ws_salesforce] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ws_salesforce] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ws_salesforce] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ws_salesforce] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ws_salesforce] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ws_salesforce] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ws_salesforce] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ws_salesforce] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ws_salesforce] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ws_salesforce] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ws_salesforce] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ws_salesforce] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ws_salesforce] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ws_salesforce] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ws_salesforce] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ws_salesforce] SET RECOVERY FULL 
GO
ALTER DATABASE [ws_salesforce] SET  MULTI_USER 
GO
ALTER DATABASE [ws_salesforce] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ws_salesforce] SET DB_CHAINING OFF 
GO
USE [ws_salesforce]
GO
/****** Object:  User [entav]    Script Date: 11/09/2018 23:40:15 ******/
CREATE USER [entav] FOR LOGIN [entav] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [entav]
GO
ALTER ROLE [db_datareader] ADD MEMBER [entav]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [entav]
GO
/****** Object:  StoredProcedure [dbo].[insertar_cuentas]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[insertar_cuentas]		
			@rut char(13),
			@razsoc char(50),
			@razsoc2 char(50),
			@dir char(50),
			@giro char(50),
			@comuna char(40),
			@ciudad char(40),
			@formap char(40),
			@diaspago int,
			@cred_apr numeric(14,2),
			@fono char(30),
			@clase1 char(40),
			@clase2 char(40),
			@clase3 char(40),
			@clase4 char(40),
			@cred_uti numeric(14,2),
			@morosid int,
			@nreguist int,
			@fecha datetime,
			@idsalesforce varchar(250)

as

begin try
begin transaction


--insert into inex_17.dbo.clien_db(nreguist,rut,razsoc,imputable,tipo,razsoc2,giro,formap,diaspago,cred_apr,clase1,clase2,clase3,clase4,cred_uti,morosid,FECHACRE,FECHULTCON,FECHAMODIF,CL_CTA,CLTESUCUR,CLTEELECTR,CTACTECLIEPROV,CLASE5,CLILP,PROD_MANT)
--values(@nreguist,@rut,@razsoc,1,1,@razsoc2,@giro,@formap,@diaspago,@cred_apr,@clase1,@clase2,@clase3,@clase4,@cred_uti,@morosid,@fecha,@fecha,@fecha,0,0,0,'','',0,'')

--insert into salesforce.dbo.Table_2(rut,razsoc,imputable,tipo,razsoc2,giro,formap,diaspago,cred_apr,clase1,clase2,clase3,clase4,cred_uti,morosid,dir,comuna,ciudad)
--values(@rut,@razsoc,1,1,@razsoc2,@giro,@formap,@diaspago,@cred_apr,@clase1,@clase2,@clase3,@clase4,@cred_uti,@morosid,@dir,@comuna,@ciudad)

COMMIT

end try
begin catch

ROLLBACK


DECLARE @ErrorMessage NVARCHAR(4000),
@ErrorSeverity INT,
@ErrorState INT,
@ErrorNumber INT,
@ErrorLine INT


SET @ErrorNumber = ERROR_NUMBER();
SET @ErrorMessage = ERROR_MESSAGE();
SET @ErrorSeverity = ERROR_SEVERITY();
SET @ErrorState = ERROR_STATE();
SET @ErrorLine = ERROR_LINE();

--insert into salesforce.dbo.Error_SQL(ErrorMessage,ErrorSeverity,ErrorState,ErrorNumber,ErrorLine)
--values(@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber,@ErrorLine)
insert into salesforce.dbo.cuenta_err(rut,razsoc,razsoc2,giro,formap,diaspago,cred_apr,clase1,clase2,clase3,clase4,cred_uti,morosid,fecha,num_error,msg_error,idsalesforce)
values(@rut,@razsoc,@razsoc2,@giro,@formap,@diaspago,@cred_apr,@clase1,@clase2,@clase3,@clase4,@cred_uti,@morosid,@fecha,@ErrorNumber,@ErrorMessage,@idsalesforce)

end catch
GO
/****** Object:  StoredProcedure [dbo].[insertar_NV]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[insertar_NV]		
			@id_cuenta varchar(50),
			@rutcliente char(13),
			@nombre_vendedor varchar(50),
			@moneda varchar(5),
			@total_neto numeric(16,2),
			@totiva int,
			@form_pago varchar(50),
			@obs_nv varchar(250),
			@obs_fact varchar(250),
			@obs_gd varchar(250),
			@obs_fav varchar(250),
			@descuento int,
			@fecha datetime,
			@estado int

as

begin try
begin transaction

declare @numnota int,
		@nreguist int,
		@numreg_perso int,
		@numreg int,
		@tasacbio int,
		@num int,
		@corr int,
		@fecha_err date,
		@codvend int

set @fecha_err=getdate()

set @fecha=getdate()

select @numreg=max(numreg)+1,@numnota=max(numnota)+1 from inx_bak.dbo.notv_db

select top 1 @nreguist=nreguist from inx_bak.dbo.clien_db where rut=@rutcliente

set @num=len(@nreguist)

select @numreg_perso=numreg from inx_bak.dbo.perso_db where rtrim(nombre)+' '+rtrim(apellido)=@nombre_vendedor

set @codvend=len(@numreg_perso)

if @moneda='CLP'
set @moneda='$'


if @moneda<>'$'
select top 1 @tasacbio=tctasa from inx_bak.dbo.tasa_db where tcmoneda=@moneda order by tcfecha desc
else
set @tasacbio=1

/*
if @num>2 and @codvend>1
insert into inx_bak.dbo.notv_db (numreg,numnota,fecha,nrutclie,nrutfact,rutfact,codvend,moneda,dctopje,totneto,dctotipo,dctopeso,tasacbio,usermodi,iva,fechamodif,obsdesp,obsfact,obsgral,ctaconta,centroco,numord,comision,numcoti,notanotv,formpago,numorden,glosacon,pagoan,dirdesp,numempvt,tipontav,totiva,docpago,sucur,procesonum,tiponv,att,aprobada,imp1,impresa,jdd,nvsuc,contarr,obra,numordc,id_imagen,docgen,imp2)
values (@numreg,@numnota,convert(datetime,@fecha,103),@nreguist,@nreguist,@rutcliente,@numreg_perso,@moneda,@descuento,@total_neto,1,0,@tasacbio,'',19,convert(datetime,@fecha,103),'',@obs_fact,'',0,0,0,0,0,@obs_nv,@form_pago,0,' ',0,'',1,1,@totiva,6,0,'',1395,'SR. Gonzalo Cifuentes',0,0,0,0,1368,0,'',' ','',0,0)
else
insert into salesforce.dbo.Notv_err(idcuenta,ErrorNumber,ErrorMessage,ErrorSeverity,ErrorState,ErrorLine,fecha)
values(@id_cuenta,1,'Hay un cliente o un vendedor que no existe',1,1,1,@fecha_err)
*/

/*
if @num>2 and @codvend>1
update todos_notasventa
set estado_sync=1
where idcuenta=@id_cuenta
*/
/*
update inx_bak.dbo.corr_db
set num=@numnota
where doc='NOTV'
*/
--select @numrecor=max(numrecor)+1 from inex_17.dbo.notde_db
/*
insert into salesforce.dbo.NV_det_prueba (numrecor,ncodart,descrip,cantidad,cantdesp,precunit,descto,bodega,nserie,ctactble,cencosto,item,fechentr,seqndvet,terminado,tax,numot,num_met,num_carr,pf,tip_tax,jdd,feppun,id_producto,sqdetoc,estado,facturable,despachable,notvcls1,notvcls2,notvcls,conceptoctble)
values(@numrecor,@ncodart,@descripcion,@cantidad,@cantidad,33,@descto,538,'',1607,348,1,convert(datetime,@fecha,103),1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)
*/
/*
insert into salesforce.dbo.aux_nv(idcuenta,numreg,sync)
values(@id_cuenta,@numreg,0)
*/
COMMIT

end try
begin catch

ROLLBACK


DECLARE @ErrorMessage NVARCHAR(4000),
@ErrorSeverity INT,
@ErrorState INT,
@ErrorNumber INT,
@ErrorLine INT


SET @ErrorNumber = ERROR_NUMBER();
SET @ErrorMessage = ERROR_MESSAGE();
SET @ErrorSeverity = ERROR_SEVERITY();
SET @ErrorState = ERROR_STATE();
SET @ErrorLine = ERROR_LINE();
/*
insert into salesforce.dbo.Notv_err(idcuenta,ErrorNumber,ErrorMessage,ErrorSeverity,ErrorState,ErrorLine,fecha)
values(@id_cuenta,@ErrorNumber,@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorLine,@fecha_err)
*/
end catch

GO
/****** Object:  StoredProcedure [dbo].[insertar_NVDET]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[insertar_NVDET]		
			@id_cuenta varchar(50),
			@id_oportunidad varchar(50),
			@id_producto varchar(50),
			@ncodart varchar(50),
			@descripcion varchar(150),
			@cantidad varchar(10),
			@precio varchar(16),
			@descuento varchar(4),
			@item varchar(3),
			@fecha datetime

as

begin try
begin transaction

declare @numrecor int,
		@seqnvdet int,
		@cod_prod varchar(20),
		@len int

select top 1 @numrecor=numreg from salesforce.dbo.aux_nv where idcuenta=@id_cuenta and sync=0 order by numreg desc

select @seqnvdet=max(seqnvdet)+1 from inx_bak.dbo.notde_db

--select @cod_prod=nreguist from inx_17.dbo.art_db where codigo=@ncodart

select @cod_prod=nreguist from inex_17.dbo.art_db where codigo=@ncodart


set @len=len(@cod_prod)



if @len>1 
if exists (select * from salesforce.dbo.aux_nv where idcuenta=@id_cuenta and sync=0)
insert into inx_bak.dbo.notde_db(numrecor,ncodart,descrip,cantidad,cantdesp,precunit,descto,bodega,nserie,ctactble,cencosto,item,fechentr,seqnvdet,terminado,tax,numot,num_met,num_carr,pf,tip_tax,jdd,feppun,id_producto,sqdetoc,estado,facturable,despachable,notvcls1,notvcls2,notvcls3)
values(@numrecor,@cod_prod,@descripcion,@cantidad,@cantidad,@precio,@descuento,538,'',1607,348,1,@fecha,@seqnvdet,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0)

if @len<1 
return 0;


COMMIT

end try
begin catch

ROLLBACK


DECLARE @ErrorMessage NVARCHAR(4000),
@ErrorSeverity INT,
@ErrorState INT,
@ErrorNumber INT,
@ErrorLine INT


SET @ErrorNumber = ERROR_NUMBER();
SET @ErrorMessage = ERROR_MESSAGE();
SET @ErrorSeverity = ERROR_SEVERITY();
SET @ErrorState = ERROR_STATE();
SET @ErrorLine = ERROR_LINE();

insert into salesforce.dbo.notde_err(idcuenta,idoportunidad,idproducto,ErrorMessage,ErrorSeverity,ErrorState,ErrorNumber,ErrorLine)
values(@id_cuenta,@id_oportunidad,@id_producto,@ErrorMessage,@ErrorSeverity,@ErrorState,@ErrorNumber,@ErrorLine)

end catch

GO
/****** Object:  StoredProcedure [dbo].[sync_contacto]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sync_contacto]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

DECLARE @id int,
	@a char(13),
	@b char(30),
	@c char(30),
	@d char(60),
	@e char(30),
	@f char(50),
	@g datetime,
	@h char(50)

DECLARE cItems CURSOR FOR

SELECT [num_contact]
      ,[rut]
      ,[nombre]
      ,[apellido]
      ,[cargo]
      ,[fono]
      ,[email]
	  ,[fecha_modificacion]
      ,[ID_Salesforce]
  FROM [salesforce].[dbo].[contacto_db]

OPEN cItems

FETCH cItems INTO @id, @a,@b,@c,@d,@e,@f,@g,@h

WHILE (@@FETCH_STATUS = 0 )

BEGIN

	IF NOT EXISTS (SELECT * FROM Contactos 
                   WHERE ID_UNICO = @id)
   BEGIN

   print 'intentado insertar '+cast(@id as varchar(5))

	INSERT INTO [dbo].[Contactos]
           ([ID_UNICO]
           ,[RUT]
           ,[NOMBRE]
           ,[APELLIDO]
           ,[CARGO]
           ,[FONO]
           ,[MAIL]
		   ,[FECHA_MOD]
           ,[ID_SALESFORCE]
           ,[OUT_ESTADO]
           ,[OUT_ID]
           ,[OUT_MSG]
           ,[OUT_FECHA]) 
	values ( @id, 
		@a,
		@b,
		@c,
		@d,
		@e,
		@f,
		@g,
		@h,
	  0,
	  '',
	  '',
	  NULL)

  END

FETCH cItems INTO @id, @a,@b,@c,@d,@e,@f,@g,@h

END

CLOSE cItems
DEALLOCATE cItems

-- fin --
RETURN 1 

END


GO
/****** Object:  StoredProcedure [dbo].[sync_cuenta]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sync_cuenta]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--return 1;
-- Declaracion de variables para el cursor

DECLARE @id int,
	@a char(13),
	@b char(50),
	@c char(50),
	@d char(50),
	@e char(40),
	@f char(40),
	@g char(40),
	@h int,
	@i numeric(14, 2),
	@j char(30),
	@k char(40),
	@l char(40),
	@m char(40),
	@n char(40),
	@o numeric(14, 2),
	@p int,
	@q datetime,
	@r char(255),
	@s int,
	@t char(255),
	@u char(255),
	@v char(255),
	@w int

DECLARE cItems CURSOR FOR

SELECT [num_cuent]
	  ,[rut]
      ,[razsoc]
      ,[dir]
      ,[giro]
      ,[comuna]
      ,[ciudad]
      ,[formap]
      ,[diaspago]
      ,[cred_apr]
      ,[fono]
      ,[clase1]
      ,[clase2]
      ,[clase3]
      ,[clase4]
      ,[cred_uti]
	  ,[morosid]
	  ,[fecha_modif]
      ,[ID_Salesforce]
      ,[principal]
      ,[dir2]
      ,[comuna2]
      ,[ciudad2]
      ,[nreguist]
  FROM [salesforce].[dbo].[cuenta_db]

OPEN cItems

FETCH cItems INTO    @id, 
@a,
@b,
@c,
@d,
@e,
@f,
@g,
@h,
@i,
@j,
@k,
@l,
@m,
@n,
@o,
@p,
@q,
@r,
@s,
@t,
@u,
@v,
@w

WHILE (@@FETCH_STATUS = 0 )

BEGIN

	IF NOT EXISTS (SELECT * FROM Cuentas 
                   WHERE ID_UNICO = @id)
   BEGIN

   print 'intentado insertar '+cast(@id as varchar(5))

	INSERT INTO [dbo].[Cuentas]
           ([ID_UNICO]
           ,[RUT]
           ,[RAZSOC]
           ,[DIR]
           ,[GIRO]
           ,[COMUNA]
           ,[CIUDAD]
           ,[FORMAP]
           ,[DIASPAGO]
           ,[CRED_APR]
           ,[FONO]
           ,[CLASE1]
           ,[CLASE2]
           ,[CLASE3]
           ,[CLASE4]
           ,[CRED_UTI]
           ,[MOROSID]
           ,[IDSALESFORCE]
           ,[FECHA_MOD]
           ,[PRINCIPAL]
           ,[DIR2]
           ,[COMUNA2]
           ,[CIUDAD2]
           ,[NREGUIST]
           ,[OUT_ESTADO]
           ,[OUT_ID]
           ,[OUT_MSG]
           ,[OUT_FECHA]) 
	values ( @id, 
		@a,
		@b,
		@c,
		@d,
		@e,
		@f,
		@g,
		@h,
		@i,
		@j,
		@k,
		@l,
		@m,
		@n,
		@o,
		@p,
		@q,
		@r,
		@s,
		@t,
		@u,
		@v,
		@w,
		0,
		'',
		'',
		NULL)

  END

FETCH cItems INTO @id, @a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k,@l,@m,@n,@o,@p,@q,@r,@s,@t,@u,@v,@w

END

CLOSE cItems
DEALLOCATE cItems

-- fin --
RETURN 1 
END


GO
/****** Object:  StoredProcedure [dbo].[Sync_NV]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sync_NV]		
			@id_cuenta varchar(50),
			@rutcliente char(13),
			@nombre_vendedor varchar(50),
			@moneda varchar(5),
			@total_neto numeric(16,2),
			@totiva int,
			@form_pago varchar(50),
			@obs_nv varchar(250),
			@obs_fact varchar(250),
			@obs_gd varchar(250),
			@obs_fav varchar(250),
			@descuento int,
			@fecha varchar(10),
			@estado int,
			@estado_desc varchar(250),
			@numoc int,
			@fecha_oc varchar(10)

as

begin try
--begin transaction

declare @numnota int,
		@numreg int,
		@nreguist int,
		@numreg_perso int,
		@tasacbio int,
		@num int,
		@codvend int,
		@max int,
		@id int

-- Recoge los codigos internos de Manager con los datos de la NV de salesforce
select @numreg=max(numreg)+1,@numnota=max(numnota)+1 from inx_bak.dbo.notv_db

select top 1 @nreguist=nreguist from inx_bak.dbo.clien_db where rut=@rutcliente
set @num=len(@nreguist)

select top 1 @nreguist=nreguist from inx_bak.dbo.clien_db where rut=@rutcliente
if not exists (select top 1 nreguist from inx_bak.dbo.clien_db where rut=@rutcliente)
begin
       RAISERROR ('Cliente no existe', 1, 1);
       return 1;
end






select @numreg_perso=numreg from inx_bak.dbo.perso_db where rtrim(nombre)+' '+rtrim(apellido)=@nombre_vendedor
set @codvend=len(@numreg_perso)

if not exists (select numreg from inx_bak.dbo.perso_db where rtrim(nombre)+' '+rtrim(apellido)=@nombre_vendedor)
begin
	   RAISERROR ('Vendedor no existe',  2,  2 );
       return 2;
end



-- Convierte el formato de moneda de salesforce al aceptado en Manager
if @moneda='CLP'
set @moneda='$'

if @moneda<>'$'
select top 1 @tasacbio=tctasa from inex_17.dbo.tasa_db where tcmoneda=@moneda order by tcfecha desc
else
set @tasacbio=1


if @form_pago='1'
set @form_pago=' '

/*

if @num>2 and @codvend>1
insert into ws_salesforce.dbo.todos_notasventa (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc)
values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),1,@estado_desc)
else
insert into ws_salesforce.dbo.todos_notasventa (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc)
values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),0,@estado_desc)


--select @id=max(id) from salesforce.dbo.notv_db


insert into salesforce.dbo.aux_nv(idcuenta,numreg,sync,total_neto,totiva,forma_de_pago,descto)
values(@id_cuenta,@numreg,0,@total_neto,@totiva,@form_pago,@descuento)


select @max=max(numreg)-1 from salesforce.dbo.aux_nv

if @num>2 and @codvend>1
update salesforce.dbo.aux_nv
set sync=1
where numreg=@max

*/
return 3;

COMMIT

end try
begin catch

--ROLLBACK


DECLARE @ErrorMessage NVARCHAR(4000),
@ErrorSeverity INT,
@ErrorState INT,
@ErrorNumber INT,
@ErrorLine INT


SET @ErrorNumber = ERROR_NUMBER();
SET @ErrorMessage = ERROR_MESSAGE();
SET @ErrorSeverity = ERROR_SEVERITY();
SET @ErrorState = ERROR_STATE();
SET @ErrorLine = ERROR_LINE();
/*
insert into salesforce.dbo.notv_db (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc,Message)
values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),0,@estado_desc,@ErrorMessage)
*/
return 0;

--select @ErrorState;

end catch

GO
/****** Object:  StoredProcedure [dbo].[Sync_NV_2]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sync_NV_2]		
			@id_cuenta varchar(50),
			@rutcliente char(13),
			@nombre_vendedor varchar(50),
			@moneda varchar(5),
			@total_neto numeric(16,2),
			@totiva int,
			@form_pago varchar(50),
			@obs_nv varchar(250),
			@obs_fact varchar(250),
			@obs_gd varchar(250),
			@obs_fav varchar(250),
			@descuento int,
			@fecha varchar(10),
			@estado int,
			@estado_desc varchar(250)

as

BEGIN TRY

declare @numnota int,
		@numreg int,
		@nreguist int,
		@numreg_perso int,
		@tasacbio int,
		@num int,
		@codvend int,
		@max int

-- Recoge los codigos internos de Manager con los datos de la NV de salesforce
select @numreg=max(numreg)+1,@numnota=max(numnota)+1 from inx_bak.dbo.notv_db

select top 1 @nreguist=nreguist from inex_17.dbo.clien_db where rut=@rutcliente
set @num=len(@nreguist)

select top 1 @nreguist=nreguist from inex_17.dbo.clien_db where rut=@rutcliente
if not exists (select top 1 nreguist from inex_17.dbo.clien_db where rut=@rutcliente)
begin
       RAISERROR ('Cliente no existe', 1, 1);
       return 1;
end;


select @numreg_perso=numreg from inex_17.dbo.perso_db where rtrim(nombre)+' '+rtrim(apellido)=@nombre_vendedor
set @codvend=len(@numreg_perso)

if not exists (select numreg from inex_17.dbo.perso_db where rtrim(nombre)+' '+rtrim(apellido)=@nombre_vendedor)
begin
	   RAISERROR ('Vendedor no existe',  2,  2 );
       return 2;
end;


-- Convierte el formato de moneda de salesforce al aceptado en Manager
if @moneda='CLP'
set @moneda='$'

if @moneda<>'$'
select top 1 @tasacbio=tctasa from inex_17.dbo.tasa_db where tcmoneda=@moneda order by tcfecha desc
else
set @tasacbio=1


if @form_pago='1'
set @form_pago=' '
/*
-- Inserción de la NV
if @num>2 and @codvend>1
insert into salesforce.dbo.notv_db (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc,Message)
values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),1,@estado_desc,'Ok')
else
insert into salesforce.dbo.notv_db (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc,Message)
values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),0,@estado_desc,'El cliente o el vendedor no existen')


insert into salesforce.dbo.aux_nv(idcuenta,numreg,sync)
values(@id_cuenta,@numreg,0)


select @max=max(numreg)-1 from salesforce.dbo.aux_nv

if @num>2 and @codvend>1
update salesforce.dbo.aux_nv
set sync=1
where numreg=@max


--insert into salesforce.dbo.notv_db (idcuenta,rutcliente,nombre_vendedor,moneda,total_neto,totiva,forma_de_pago,obs_nv,obs_factura,Obs_GD,Obs_FAV,Descto,Fecha,estado_sync,estado_desc,Message)
--values (@id_cuenta,@rutcliente,@nombre_vendedor,@moneda,@total_neto,@totiva,@form_pago,@obs_nv,@obs_fact,@obs_gd,@obs_fav,@descuento,convert(date,@fecha),0,@estado_desc,'ERROR')
*/

END TRY
BEGIN CATCH
    DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT;
    DECLARE @ErrorState INT;

    SELECT @ErrorMessage = ERROR_MESSAGE(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE();

    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH;
GO
/****** Object:  StoredProcedure [dbo].[sync_producto]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sync_producto]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
DECLARE @a char(30),
	@b char(45),
	@c char(40),
	@d char(40),
	@e char(40),
	@f char(40),
	@g char(40),
	@h numeric(16, 2),
	@i numeric(16, 2),
	@j numeric(10, 2),
	@k int

DECLARE cItems CURSOR FOR

SELECT [CODIGO]
      ,[nombre]
      ,[clase1]
      ,[clase2]
      ,[clase3]
      ,[clase4]
      ,[monevta]
      ,[precvta]
      ,[costorep]
      ,[facequi]
      ,[stock]
  FROM [salesforce].[dbo].[vista_producto]

OPEN cItems

FETCH cItems INTO  @a,
@b,
@c,
@d,
@e,
@f,
@g,
@h,
@i,
@j,
@k

WHILE (@@FETCH_STATUS = 0 )

BEGIN

	IF NOT EXISTS (SELECT * FROM Productos 
                   WHERE CODIGO = @a)
   BEGIN

   print 'intentado insertar '+@a

	INSERT INTO [dbo].[Productos]
           ([CODIGO]
           ,[NOMBRE]
           ,[CLASE1]
           ,[CLASE2]
           ,[CLASE3]
           ,[CLASE4]
           ,[MONEVTA]
           ,[PRECVTA]
           ,[COSTOREP]
           ,[FACEQUI]
           ,[STOCK]
           ,[OUT_ESTADO]
           ,[OUT_ID]
           ,[OUT_MSG]
           ,[OUT_FECHA])
	values ( @a,
		@b,
		@c,
		@d,
		@e,
		@f,
		@g,
		@h,
		@i,
		@j,
		@k,
	  0,
	  '',
	  '',
	  NULL)

  END

FETCH cItems INTO @a,@b,@c,@d,@e,@f,@g,@h,@i,@j,@k

END

CLOSE cItems
DEALLOCATE cItems

-- fin --
RETURN 1 
END


GO
/****** Object:  Table [dbo].[Contactos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contactos](
	[ID_CONTACTO] [int] IDENTITY(1,1) NOT NULL,
	[ID_UNICO] [int] NULL,
	[RUT] [varchar](13) NULL,
	[NOMBRE] [varchar](30) NULL,
	[APELLIDO] [varchar](30) NULL,
	[CARGO] [varchar](60) NULL,
	[FONO] [varchar](30) NULL,
	[MAIL] [varchar](50) NULL,
	[FECHA_MOD] [datetime] NULL,
	[ID_SALESFORCE] [nchar](40) NULL,
	[OUT_ESTADO] [int] NULL,
	[OUT_ID] [varchar](100) NULL,
	[OUT_MSG] [text] NULL,
	[OUT_FECHA] [datetime] NULL,
 CONSTRAINT [PK_Contactos] PRIMARY KEY CLUSTERED 
(
	[ID_CONTACTO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Cuentas]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cuentas](
	[ID_CUENTA] [int] IDENTITY(1,1) NOT NULL,
	[ID_UNICO] [int] NULL,
	[RUT] [varchar](13) NULL,
	[RAZSOC] [varchar](50) NULL,
	[DIR] [varchar](50) NULL,
	[GIRO] [varchar](50) NULL,
	[COMUNA] [varchar](40) NULL,
	[CIUDAD] [varchar](40) NULL,
	[FORMAP] [varchar](40) NULL,
	[DIASPAGO] [int] NULL,
	[CRED_APR] [numeric](14, 2) NULL,
	[FONO] [varchar](30) NULL,
	[CLASE1] [varchar](40) NULL,
	[CLASE2] [varchar](40) NULL,
	[CLASE3] [varchar](40) NULL,
	[CLASE4] [varchar](40) NULL,
	[CRED_UTI] [numeric](14, 2) NULL,
	[MOROSID] [int] NULL,
	[IDSALESFORCE] [varchar](50) NULL,
	[FECHA_MOD] [datetime] NULL,
	[PRINCIPAL] [int] NULL,
	[DIR2] [varchar](500) NULL,
	[COMUNA2] [varchar](50) NULL,
	[CIUDAD2] [varchar](50) NULL,
	[NREGUIST] [int] NULL,
	[OUT_ESTADO] [int] NULL,
	[OUT_ID] [varchar](100) NULL,
	[OUT_MSG] [text] NULL,
	[OUT_FECHA] [datetime] NULL,
 CONSTRAINT [PK_Cuentas] PRIMARY KEY CLUSTERED 
(
	[ID_CUENTA] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Productos](
	[ID_PRODUCTO] [int] IDENTITY(1,1) NOT NULL,
	[CODIGO] [varchar](30) NULL,
	[NOMBRE] [varchar](45) NULL,
	[CLASE1] [varchar](40) NULL,
	[CLASE2] [varchar](40) NULL,
	[CLASE3] [varchar](40) NULL,
	[CLASE4] [varchar](40) NULL,
	[MONEVTA] [varchar](40) NULL,
	[PRECVTA] [numeric](16, 2) NULL,
	[COSTOREP] [numeric](16, 2) NULL,
	[FACEQUI] [numeric](10, 2) NULL,
	[STOCK] [int] NULL,
	[OUT_ESTADO] [int] NULL,
	[OUT_ID] [varchar](100) NULL,
	[OUT_MSG] [text] NULL,
	[OUT_FECHA] [datetime] NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[ID_PRODUCTO] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Todos_Contactos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Todos_Contactos](
	[ID_NCONT] [int] IDENTITY(1,1) NOT NULL,
	[IDSALESFORCE] [varchar](255) NULL,
	[IDCUENTA] [varchar](255) NULL,
	[NCONT_CUENTA] [varchar](255) NULL,
	[NCONT_EMAIL] [varchar](255) NULL,
	[NCONT_NOMBRE] [varchar](255) NULL,
	[NCONT_APELLIDO] [varchar](255) NULL,
	[NCONT_FONO] [varchar](255) NULL,
	[NCONT_CARGO] [varchar](255) NULL,
	[NCONT_ULTIMAF] [varchar](255) NULL,
 CONSTRAINT [PK_Todos_Contactos] PRIMARY KEY CLUSTERED 
(
	[ID_NCONT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Todos_Cuentas]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Todos_Cuentas](
	[ID_NCUENT] [int] IDENTITY(1,1) NOT NULL,
	[IDSALESFORCE] [varchar](255) NULL,
	[NCUENT_NOMBRE] [varchar](255) NULL,
	[NCUENT_AREA] [varchar](255) NULL,
	[NCUENT_CAPROB] [varchar](255) NULL,
	[NCUENT_CUTILIZ] [varchar](255) NULL,
	[NCUENT_RAZON] [varchar](255) NULL,
	[NCUENT_RUT] [varchar](255) NULL,
	[NCUENT_GIRO] [varchar](255) NULL,
	[NCUENT_TIPO] [varchar](255) NULL,
	[NCUENT_ZONA] [varchar](255) NULL,
	[NCUENT_NAC] [varchar](255) NULL,
	[NCUENT_DIVISION] [varchar](255) NULL,
	[NCUENT_FPAGO] [varchar](255) NULL,
	[NCUENT_DPAGO] [varchar](255) NULL,
	[NCUENT_DICOM] [varchar](255) NULL,
	[NCUENT_MOROSIDAD] [varchar](255) NULL,
	[NCUENT_IMPUTABLE] [bit] NULL,
	[NCUENT_ESMATRIZ] [bit] NULL,
	[NCUENT_DIRE] [varchar](500) NULL,
	[NCUENT_COMUNA] [varchar](255) NULL,
	[NCUENT_CIUDAD] [varchar](255) NULL,
	[NCUENT_ULTIMAF] [varchar](255) NULL,
	[NCUENT_DESP_DIRE] [varchar](500) NULL,
	[NCUENT_DESP_CIUDAD] [varchar](255) NULL,
	[NCUENT_DESP_COMUNA] [varchar](255) NULL,
 CONSTRAINT [PK_Todos_Cuentas] PRIMARY KEY CLUSTERED 
(
	[ID_NCUENT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Todos_NotasVenta]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Todos_NotasVenta](
	[IdCuenta] [varchar](50) NOT NULL,
	[RutCliente] [varchar](500) NOT NULL,
	[nombre_vendedor] [varchar](500) NOT NULL,
	[Moneda] [varchar](500) NOT NULL,
	[Total_neto] [float] NOT NULL,
	[Totiva] [int] NOT NULL,
	[Forma_de_Pago] [varchar](500) NOT NULL,
	[Obs_NV] [varchar](500) NOT NULL,
	[Obs_Factura] [varchar](500) NOT NULL,
	[Obs_GD] [varchar](500) NOT NULL,
	[Obs_FAV] [varchar](500) NOT NULL,
	[Descto] [float] NOT NULL,
	[Fecha] [date] NOT NULL,
	[estado_sync] [int] NOT NULL,
	[estado_desc] [varchar](50) NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Todos_NV_Productos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Todos_NV_Productos](
	[IdCuenta] [varchar](255) NOT NULL,
	[IdOportunidad] [varchar](255) NOT NULL,
	[Idproducto] [varchar](255) NOT NULL,
	[ncodart] [varchar](255) NOT NULL,
	[descripcion] [varchar](255) NOT NULL,
	[cantidad] [varchar](255) NOT NULL,
	[precio] [varchar](255) NOT NULL,
	[descuento] [varchar](255) NOT NULL,
	[item] [varchar](255) NOT NULL,
	[fecha_entrega] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Todos_Productos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Todos_Productos](
	[ID_NPROD] [int] IDENTITY(1,1) NOT NULL,
	[IDSALESFORCE] [varchar](255) NULL,
	[NPROD_NOMBRE] [varchar](255) NULL,
	[NPROD_CODIGO] [varchar](255) NULL,
	[NPROD_CLASE1] [varchar](255) NULL,
	[NPROD_MONEVTA] [varchar](255) NULL,
	[NPROD_PRECVTA] [varchar](255) NULL,
	[MNG_ESTADO] [int] NULL,
	[MNG_GLOSA] [varchar](50) NULL,
 CONSTRAINT [PK_Todos_Productos] PRIMARY KEY CLUSTERED 
(
	[ID_NPROD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[cuentas_y_contactos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[cuentas_y_contactos]
AS
SELECT        dbo.Todos_Cuentas.NCUENT_NOMBRE AS CUENTA, COUNT(dbo.Todos_Contactos.ID_NCONT) AS N_CONTACTOSQ
FROM            dbo.Todos_Contactos INNER JOIN
                         dbo.Todos_Cuentas ON dbo.Todos_Contactos.IDCUENTA = dbo.Todos_Cuentas.IDSALESFORCE
GROUP BY dbo.Todos_Cuentas.NCUENT_NOMBRE

GO
/****** Object:  View [dbo].[externos_contactos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[externos_contactos]
AS
SELECT        salesforce.dbo.contacto_db.*
FROM            salesforce.dbo.contacto_db

GO
/****** Object:  View [dbo].[externos_cuentas]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[externos_cuentas]
AS
SELECT        salesforce.dbo.cuenta_db.*
FROM            salesforce.dbo.cuenta_db

GO
/****** Object:  View [dbo].[externos_productos]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[externos_productos]
AS
SELECT        salesforce.dbo.vista_producto.*
FROM            salesforce.dbo.vista_producto

GO
/****** Object:  View [dbo].[listar_contactos_q]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[listar_contactos_q]
AS
SELECT        dbo.Contactos.*
FROM            dbo.Contactos
WHERE        (OUT_ESTADO = 0)



GO
/****** Object:  View [dbo].[listar_cuentas_q]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[listar_cuentas_q]
AS
SELECT        ID_CUENTA, RUT, RAZSOC, DIR, GIRO, COMUNA, CIUDAD, FORMAP, DIASPAGO, CRED_APR, FONO, CLASE1, CLASE2, CLASE3, CLASE4, CRED_UTI, MOROSID, OUT_ESTADO, OUT_ID, OUT_MSG, 
                         OUT_FECHA
FROM            dbo.Cuentas
WHERE        (OUT_ESTADO = 0)



GO
/****** Object:  View [dbo].[Listar_Notas_Venta]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Listar_Notas_Venta]
AS
SELECT        dbo.Todos_NV_Productos.Idproducto, dbo.Todos_NV_Productos.ncodart, dbo.Todos_NV_Productos.descripcion
FROM            dbo.Todos_NotasVenta INNER JOIN
                         dbo.Todos_NV_Productos ON dbo.Todos_NotasVenta.IdCuenta = dbo.Todos_NV_Productos.IdCuenta

GO
/****** Object:  View [dbo].[listar_notasventa_q]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[listar_notasventa_q]
AS
SELECT        dbo.Todos_NotasVenta.IdCuenta, dbo.Todos_NotasVenta.estado_sync, dbo.Todos_NotasVenta.estado_desc, dbo.Todos_NV_Productos.IdOportunidad
FROM            dbo.Todos_NotasVenta INNER JOIN
                         dbo.Todos_NV_Productos ON dbo.Todos_NotasVenta.IdCuenta = dbo.Todos_NV_Productos.IdCuenta
WHERE        (dbo.Todos_NotasVenta.estado_sync = 4) OR
                         (dbo.Todos_NotasVenta.estado_sync = 5)

GO
/****** Object:  View [dbo].[listar_productos_q]    Script Date: 11/09/2018 23:40:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[listar_productos_q]
AS
SELECT        dbo.Productos.*
FROM            dbo.Productos
WHERE        (OUT_ESTADO = 0)



GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Todos_Productos]    Script Date: 11/09/2018 23:40:16 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Todos_Productos] ON [dbo].[Todos_Productos]
(
	[IDSALESFORCE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Todos_NotasVenta] ADD  CONSTRAINT [DF_Todos_NotasVenta_estado_sync]  DEFAULT ((0)) FOR [estado_sync]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[32] 4[30] 2[4] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Todos_Contactos"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Todos_Cuentas"
            Begin Extent = 
               Top = 6
               Left = 285
               Bottom = 136
               Right = 511
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 3345
         Width = 4170
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 12
         Column = 2325
         Alias = 2415
         Table = 2685
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'cuentas_y_contactos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'cuentas_y_contactos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[8] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "contacto_db (salesforce.dbo)"
            Begin Extent = 
               Top = 8
               Left = 149
               Bottom = 138
               Right = 358
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_contactos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_contactos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[41] 4[21] 2[12] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "cuenta_db (salesforce.dbo)"
            Begin Extent = 
               Top = 121
               Left = 210
               Bottom = 251
               Right = 419
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_cuentas'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_cuentas'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "vista_producto (salesforce.dbo)"
            Begin Extent = 
               Top = 47
               Left = 179
               Bottom = 177
               Right = 388
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_productos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'externos_productos'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[54] 4[8] 2[10] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Todos_NotasVenta"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 258
               Right = 312
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Todos_NV_Productos"
            Begin Extent = 
               Top = 16
               Left = 413
               Bottom = 265
               Right = 660
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Listar_Notas_Venta'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Listar_Notas_Venta'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Todos_NotasVenta"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 211
               Right = 438
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Todos_NV_Productos"
            Begin Extent = 
               Top = 31
               Left = 563
               Bottom = 161
               Right = 772
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 2475
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'listar_notasventa_q'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'listar_notasventa_q'
GO
USE [master]
GO
ALTER DATABASE [ws_salesforce] SET  READ_WRITE 
GO
