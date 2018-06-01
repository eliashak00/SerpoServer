CREATE TABLE IF NOT EXISTS spo_users(
  user_id INT PRIMARY KEY,
  user_email VARCHAR(50) NOT NULL,
  user_nick VARCHAR(50) NOT NULL,
  user_password VARCHAR(500) NOT NULL,
  user_salt VARCHAR(500) NOT NULL,
  user_avatar VARCHAR(500) NOT NULL,
  user_registerd DATE NOT NULL
);
CREATE TABLE IF NOT EXISTS spo_sites(
  site_id INT NOT NULL,
  site_domain VARCHAR(100) NOT NULL
);
CREATE TABLE IF NOT EXISTS spo_pages(
  page_id INT PRIMARY KEY ,
  page_route VARCHAR(20) NOT NULL,
  page_script VARCHAR(2000),
  page_view VARCHAR(2000),
  page_method INT NOT NULL,
  page_response INT NOT NULL,
  page_site INT FOREIGN KEY REFERENCES spo_sites(site_id)
);
CREATE TABLE IF NOT EXISTS spo_stats(
  stat_id INT PRIMARY KEY,
  stat_site INT FOREIGN KEY REFERENCES spo_sites(site_id),
  
);
CREATE TABLE IF NOT EXISTS spo_days(
  day_id INT PRIMARY KEY,
  day_stat INT FOREIGN KEY REFERENCES spo_stats(stat_id),
  day_views INT NOT NULL
  
);
CREATE TABLE IF NOT EXISTS spo_services(
  service_id INT PRIMARY KEY,
  service_name VARCHAR(100) NOT NULL,
  service_script VARCHAR(5000) NOT NULL
);
CREATE TABLE IF NOT EXISTS spo_service_rel(
  sr_id INT PRIMARY KEY,
  sr_site INT REFERENCES spo_sites(site_id),
  sr_service INT REFERENCES spo_sites(site_id)
);
CREATE VIEW 