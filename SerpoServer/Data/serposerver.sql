
CREATE TABLE IF NOT EXISTS spo_users
(
    user_id int PRIMARY KEY AUTO_INCREMENT ,
    user_email VARCHAR(50)  ,
    user_nick VARCHAR(100)  ,
    user_password VARCHAR(200)  ,
    user_salt VARCHAR(100)  ,
      user_avatar VARCHAR(500)  ,
    user_registerd DATETIME,
    user_site INT,
    user_role INT
);
CREATE TABLE IF NOT EXISTS spo_sites(
  site_id INT  AUTO_INCREMENT PRIMARY KEY,
  site_domain VARCHAR(100),
  site_name VARCHAR(100),
  site_ssl INT
);
CREATE TABLE IF NOT EXISTS spo_pages(
  page_id INT  AUTO_INCREMENT PRIMARY KEY,
  page_route VARCHAR(20)  ,
  page_script VARCHAR(2000) ,
  page_view VARCHAR(2000) ,
  page_methods INT  ,
  page_response INT  ,
  page_crud INT REFERENCES spo_cruds(crud_id),
  page_site INT REFERENCES spo_sites(site_id)
);
CREATE TABLE IF NOT EXISTS spo_stats(
  stat_id INT  AUTO_INCREMENT PRIMARY KEY ,
  stat_site INT REFERENCES spo_sites(site_id)

);
CREATE TABLE IF NOT EXISTS spo_modules(
  module_id INT AUTO_INCREMENT PRIMARY KEY,
  module_name VARCHAR(100),
  module_active INT,
  module_pos INT,
  module_lat INT,
  module_js VARCHAR(100)
);
CREATE TABLE IF NOT EXISTS spo_days(
  day_id INT AUTO_INCREMENT PRIMARY KEY ,
  day_views INT,
  day_site INT REFERENCES spo_sites(site_id),
  day_date DATE

);
CREATE TABLE IF NOT EXISTS spo_services(
  service_id INT  AUTO_INCREMENT PRIMARY KEY,
  service_name VARCHAR(100)  ,
  service_script VARCHAR(5000)  
);
CREATE TABLE IF NOT EXISTS spo_service_rel(
  sr_id INT  AUTO_INCREMENT PRIMARY KEY ,
  sr_site INT REFERENCES spo_sites(site_id),
  sr_service INT REFERENCES spo_sites(site_id)
);
CREATE TABLE IF NOT EXISTS spo_cruds(
  crud_id INT  AUTO_INCREMENT PRIMARY KEY ,
  crud_table VARCHAR(100),
   crud_struct VARCHAR(1000),
  crud_auth INT,
  crud_password VARCHAR(200),
  crud_json LONGTEXT
);