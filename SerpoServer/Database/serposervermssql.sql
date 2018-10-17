
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_users')
    BEGIN
      CREATE TABLE spo_users
      (
        user_id            INT PRIMARY KEY IDENTITY,
        user_email         VARCHAR(50),
        user_nick          VARCHAR(100),
        user_password      VARCHAR(200),
        user_salt VARCHAR(100),
        user_registerd     DATETIME,
        user_avatar        VARCHAR(500),
        user_role INT,
        user_site INT
      );
    END
    
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_sites')
    BEGIN
CREATE TABLE spo_sites(
  site_id INT  PRIMARY KEY IDENTITY ,
  site_domain VARCHAR(100),
  site_name VARCHAR(100),
  site_ssl INT
);
END

  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_pages')
    BEGIN
CREATE TABLE  spo_pages(
  page_id INT PRIMARY KEY IDENTITY,
  page_route VARCHAR(20)  ,
  page_script VARCHAR(2000) ,
  page_view VARCHAR(2000) ,
  page_method INT  ,
  page_response INT  ,
  page_crud INT REFERENCES spo_cruds(crud_id),
  page_site INT REFERENCES spo_sites(site_id)
);
END

  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_stats')
    BEGIN
CREATE TABLE  spo_stats(
  stat_id INT PRIMARY KEY IDENTITY,
  stat_site INT REFERENCES spo_sites(site_id)

);
END

  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_days')
    BEGIN
CREATE TABLE  spo_days(
  day_id INT  PRIMARY KEY IDENTITY,
  day_date DATE,
  day_views INT,
  day_site INT REFERENCES spo_sites(site_id)

);
END
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_modules')
    BEGIN
CREATE TABLE spo_modules(
  module_id INT AUTO_INCREMENT PRIMARY KEY,
  module_name VARCHAR(100),
  module_active INT,
  module_pos INT,
  module_lat INT,
  module_js VARCHAR(100)
);
END 
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_services')
    BEGIN
CREATE TABLE spo_services(
  service_id INT PRIMARY KEY IDENTITY,
  service_name VARCHAR(100)  ,
  service_script VARCHAR(5000)  
);
END 
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_service_rel')
    BEGIN
CREATE TABLE spo_service_rel(
  sr_id INT PRIMARY KEY IDENTITY,
  sr_site INT REFERENCES spo_sites(site_id),
  sr_service INT REFERENCES spo_sites(site_id)
);
END
  IF NOT EXISTS(SELECT *
                FROM sysobjects
                WHERE Name = 'spo_cruds')
    BEGIN
CREATE TABLE spo_cruds(
  crud_id INT PRIMARY KEY IDENTITY,
  crud_table VARCHAR(100),
  crud_struct VARCHAR(1000),
  crud_json VARCHAR(MAX),
  crud_auth INT,
  crud_password VARCHAR(200)
);
END
CREATE PROCEDURE remove_crud
  @id INT
AS
    DELETE FROM spo_pages WHERE page_crud = @id;
    DELETE FROM spo_cruds WHERE crud_id = @id;
GO