
CREATE TABLE IF NOT EXISTS spo_users
(
    user_id int PRIMARY KEY AUTO_INCREMENT ,
    user_email VARCHAR(50)  ,
    user_nick VARCHAR(100)  ,
    user_password VARCHAR(200)  ,
    user_salt VARCHAR(100)  ,
      user_avatar VARCHAR(500)  ,
    user_registerd DATETIME
);
CREATE TABLE IF NOT EXISTS spo_sites(
  site_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  site_domain VARCHAR(100)  
);
CREATE TABLE IF NOT EXISTS spo_pages(
  page_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  page_route VARCHAR(20)  ,
  page_script VARCHAR(2000) ,
  page_view VARCHAR(2000) ,
  page_method INT  ,
  page_response INT  ,
  page_site INT REFERENCES spo_sites(site_id)
);
CREATE TABLE IF NOT EXISTS spo_stats(
  stat_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  stat_site INT REFERENCES spo_sites(site_id)

);
CREATE TABLE IF NOT EXISTS spo_days(
  day_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  day_stat INT REFERENCES spo_stats(stat_id),
  day_views INT  

);
CREATE TABLE IF NOT EXISTS spo_services(
  service_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  service_name VARCHAR(100)  ,
  service_script VARCHAR(5000)  
);
CREATE TABLE IF NOT EXISTS spo_service_rel(
  sr_id INT  AUTO_INCREMENT PRIMARY KEY AUTO_INCREMENT,
  sr_site INT REFERENCES spo_sites(site_id),
  sr_service INT REFERENCES spo_sites(site_id)
);