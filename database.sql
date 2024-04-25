-- Bootcamp SDA Softwere Development C# - Integrify
-- Project: sda-online-2-csharp-backend_teamwork
-- Name: E-commerce Application API
-- URL: https://github.com/Integrify-Finland/sda-online-2-csharp-backend_teamwork
-- Team: The Legends ✨
-- Team members:
--      @Mohkhasa
--      @ahadND71
--      @EnasBatarfi
--      @ShahadDraim
-- Date: 24 Apr 2024


-- 
--      Tables creation code: 
--      [use this order for creating these tables]
-- 

--
create table customer (
    customer_id   serial   primary key,
    first_name    varchar(100) not null,
    last_name     varchar(100) not null,
    email         varchar(100) unique not null,
    password      varchar(100) not null,
    mobile        varchar(25) not null,
    is_banned     boolean default false,
    created_at    timestamp default current_timestamp
);

-- 
create table admin (
    admin_id      serial primary key,
    first_name    varchar(100) not null,
    last_name     varchar(100) not null,
    email         varchar(100) unique not null,
    password      varchar(100) not null,
    mobile        varchar(25) not null,
    created_at    timestamp default current_timestamp
);

-- 
create table address (
    address_id     serial   primary key,
    customer_id    int,
    name           varchar(100) not null,
    address_line1  varchar(100) not null,
    address_line2  varchar(100),
    country        varchar(100) not null,
    province       varchar(100) not null,
    city           varchar(100) not null,
    zip_code       varchar(25),
    foreign key(customer_id) references customer(customer_id),
    constraint unique_address_name unique (customer_id, name)
);

-- 
create table category (
    category_id    serial  primary key,
    admin_id       int,
    name           varchar(100) unique not null,
    slug           varchar(100) unique not null,
    description    text not null,
    created_at     timestamp default current_timestamp,
    foreign key(admin_id) references admin(admin_id)
);

-- 
create table product (
    product_id       serial  primary key,
    category_id      int,
    admin_id         int,
    name             varchar(100) unique not null,
    slug             varchar(100) unique not null,
    description      text not null,
    price            decimal(10,2) not null,
    SKU              varchar(100) not null,
    stock_quantity   int not null,
    img_url          varchar(250) not null,
    created_at       timestamp default current_timestamp,
    updated_at       timestamp default current_timestamp,
    foreign key(category_id) references category(category_id),
    foreign key(admin_id) references admin(admin_id)
);

-- 
create table "order" (
    order_id         serial  primary key,
    customer_id      int,
    address_id       int,
    date             timestamp default current_timestamp,
    total_amount     decimal(10,2) not null,
    status           varchar(100) default 'pending',
    foreign key(customer_id) references customer(customer_id),
    foreign key(address_id) references address(address_id)
);

-- 
create table order_product (
    order_item_id     serial  primary key,
    order_id          int,
    product_id        int,
    quantity          int default 1,
    price             decimal(10,2) not null,
    foreign key(order_id) references "order"(order_id),
    foreign key(product_id) references product(product_id)
);

-- 
create table review (
    review_id       serial primary key,
    order_id        int not null, 
    customer_id     int not null,
    product_id      int not null,
    rating          int check (rating >= 1 and rating <= 5), 
    comment         text,
    review_date     timestamp default current_timestamp,
    status          varchar(50) default 'pending',
    is_anonymous    boolean default false,
    foreign key (product_id) references product(product_id),
    foreign key (customer_id) references customer(customer_id),
    foreign key(order_id) references "order"(order_id)
);


---------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------

-- 
--      Tables data insertion: 
-- 

-- 
--      admin 
-- 
-- Insert data into admin table 
insert into admin (first_name, last_name, email,password, mobile)
values
('Enas', 'Batarfi', 'enas@example.com','123456', '0536513037'),
('Mohammed', 'Alkhamis', 'mohammed@example.com', '123456','0555555777'),
('Ahad', 'Aldossari', 'ahad@example.com','123456', '05888222444'),
('Shahad', 'Draim', 'shahad@example.com','123456', '05378965478');


-- 
--      customer 
-- 
-- Insert data into customer table 
insert into customer (first_name, last_name, email, password, mobile)
values
('Ahmed', 'Ali', 'ahmed@example.com','123456', '123456789'),
('Fatima', 'Mohammad', 'fatima@example.com', '123456','234567890'),
('Youssef', 'Ibrahim', 'youssef@example.com','123456', '345678901'),
('Amina', 'Hassan', 'amina@example.com','123456', '456789012'),
('Omar', 'Khalid', 'omar@example.com', '123456','567890123'),
('Sara', 'Ahmad', 'sara@example.com', '123456','678901234'),
('Ali', 'Abdullah', 'ali@example.com','123456', '789012345'),
('Huda', 'Salem', 'huda@example.com','123456', '890123456'),
('Khaled', 'Nour', 'khaled@example.com','123456', '901234567'),
('Layla', 'Rashid', 'layla@example.com', '123456','1234567890');


-- 
--      address 
-- 
-- Insert data into address table 
insert into address (customer_id, name, address_line1, address_line2, country, province, city, zip_code)
values
(1, 'Home', '123 King Fahd Road', 'Apartment 5A', 'Saudi Arabia', 'Riyadh', 'Riyadh', '12345'),
(1, 'Work', '456 Olaya Street', 'Building B, Floor 10', 'Saudi Arabia', 'Riyadh', 'Riyadh', '23456'),
(1, 'Dad House', '789 Al Dabbab Street', null, 'Saudi Arabia', 'Riyadh', 'Riyadh', '34567'),
(2, 'Home', '789 Tahlia Street', 'Apartment 3', 'Saudi Arabia', 'Jeddah', 'Jeddah', '45678'),
(2, 'Work', '456 Al Hamra Street', 'Office 202', 'Saudi Arabia', 'Jeddah', 'Jeddah', '56789'),
(2, 'lolo House', '321 Corniche Road', 'Villa 15', 'Saudi Arabia', 'Jeddah', 'Jeddah', '67890'),
(3, 'Home', '321 Prince Sultan Street', null, 'Saudi Arabia', 'Jeddah', 'Jeddah', '78901'),
(3, 'Work', '654 King Abdulaziz Street', 'Tower 2, Floor 15', 'Saudi Arabia', 'Riyadh', 'Riyadh', '89012'),
(4, 'Home', '987 Al Khaleej Road', 'Apartment 12B', 'Saudi Arabia', 'Dammam', 'Dammam', '90123'),
(4, 'Ahmed House', '147 Al Rashid Street', null, 'Saudi Arabia', 'Dammam', 'Dammam', '01234'),
(5, 'Home', '147 Imam Saud Street', null, 'Saudi Arabia', 'Riyadh', 'Riyadh', '12345'),
(6, 'Home', '258 Olaya Street', 'The Galleria, Unit 301', 'Saudi Arabia', 'Jeddah', 'Jeddah', '23456'),
(6, 'Friend''s House', '369 King Fahd Road', 'Villa 10', 'Saudi Arabia', 'Jeddah', 'Jeddah', '34567'),
(7, 'Home', '369 Al Madinah Road', 'Block A, Apartment 7', 'Saudi Arabia', 'Makkah', 'Makkah', '45678'),
(8, 'Home', '741 King Fahd Road', null, 'Saudi Arabia', 'Riyadh', 'Riyadh', '56789'),
(8, 'Work', '852 Othman Bin Affan Road', 'Tower 1, Floor 20', 'Saudi Arabia', 'Jeddah', 'Jeddah', '67890'),
(9, 'Home', '963 Al Murabba Street', 'Block D', 'Saudi Arabia', 'Riyadh', 'Riyadh', '78901'),
(9, 'Home2', '741 Tahlia Street', 'Apartment 8', 'Saudi Arabia', 'Riyadh', 'Riyadh', '89012'),
(10, 'Home', '123 Tahlia Street', null, 'Saudi Arabia', 'Jeddah', 'Jeddah', '90123'),
(10, 'Work', '456 King Abdullah Road', 'Apartment 2C', 'Saudi Arabia', 'Riyadh', 'Riyadh', '01234'),
(10, 'Parents House', '789 Al Madinah Road', 'Villa 5', 'Saudi Arabia', 'Jeddah', 'Jeddah', '12345');


-- 
--      category 
-- 
-- Insert data into category table 
insert into category (admin_id, name,slug, description)
values
(1, 'Laptops','laptops', 'Portable computers for work and entertainment'),
(2, 'Mobile Phones', 'mobile-phones','Smartphones for communication and productivity'),
(2, 'Earphones','earphones', 'Audio devices for listening to music and calls'),
(4, 'Tablets','tablets', 'Portable touchscreen devices for various tasks'),
(3, 'Smart Watches','smart-watches', 'Wearable devices with various functionalities'),
(3, 'Cameras','cameras', 'Capture precious moments with high-quality cameras'),
(1, 'Televisions','televisions', 'Entertainment hubs for movies, shows, and gaming'),
(4, 'Other','other', 'Various products that do not fit into other categories');


-- 
--      product 
-- 
-- Insert data into product table 
insert into product (category_id, admin_id, name, slug, description, price, SKU, stock_quantity, img_url)
values
-- Laptops
(1, 1, 'Dell XPS 13', 'dell-xps-13', 'Ultra-thin laptop with 4K display', 1500.00, 'dellxps13', 20, 'https://example.com/dell_xps_13.jpg'),
(1, 2, 'MacBook Pro', 'macbook-pro', 'Powerful laptop for professionals', 2000.00, 'macbookpro', 15, 'https://example.com/macbook_pro.jpg'),
(1, 4, 'HP Spectre x360', 'hp-spectre-x360', 'Convertible laptop with touchscreen', 1300.00, 'hpspectre360', 25, 'https://example.com/hp_spectre_x360.jpg'),

-- Mobiles
(2, 2, 'iPhone 13 Pro', 'iphone-13-pro', 'Latest iPhone with A15 Bionic chip', 1200.00, 'iphone13pro', 30, 'https://example.com/iphone_13_pro.jpg'),
(2, 3, 'Samsung Galaxy S22 Ultra', 'samsung-galaxy-s22-ultra', 'Flagship Android smartphone with 108MP camera', 1300.00, 'galaxys22ultra', 25, 'https://example.com/samsung_galaxy_s22_ultra.jpg'),
(2, 1, 'Google Pixel 6 Pro', 'google-pixel-6-pro', 'Pixel perfect photography with Tensor chip', 1000.00, 'pixel6pro', 35, 'https://example.com/google_pixel_6_pro.jpg'),

-- Earphones
(3, 3, 'AirPods Pro', 'airpods-pro', 'Active Noise Cancellation for immersive sound', 250.00, 'airpodspro', 40, 'https://example.com/airpods_pro.jpg'),
(3, 2, 'Sony WH-1000XM4', 'sony-wh-1000xm4', 'Wireless Noise Cancelling Headphones', 300.00, 'wh1000xm4', 30, 'https://example.com/sony_wh1000xm4.jpg'),

-- Tablets
(4, 1, 'iPad Air', 'ipad-air', 'Powerful tablet with A14 Bionic chip', 800.00, 'ipadair', 20, 'https://example.com/ipad_air.jpg'),
(4, 1, 'Samsung Galaxy Tab S7', 'samsung-galaxy-tab-s7', 'Premium Android tablet with S Pen', 700.00, 'galaxytab7', 15, 'https://example.com/galaxy_tab_s7.jpg'),
(4, 4, 'Microsoft Surface Pro 7', 'microsoft-surface-pro-7', 'Versatile tablet with laptop-class performance', 900.00, 'surfacepro7', 25, 'https://example.com/surface_pro_7.jpg'),

-- Smartwatches
(5, 4, 'Apple Watch Series 7', 'apple-watch-series-7', 'The ultimate watch for a healthier life', 400.00, 'applewatch7', 30, 'https://example.com/apple_watch_series_7.jpg'),
(5, 2, 'Samsung Galaxy Watch 4', 'samsung-galaxy-watch-4', 'Advanced smartwatch with health tracking', 350.00, 'galaxywatch4', 25, 'https://example.com/galaxy_watch_4.jpg'),
(5, 3, 'Fitbit Versa 3', 'fitbit-versa-3', 'Fitness tracker with built-in GPS', 200.00, 'fitbitversa3', 40, 'https://example.com/fitbit_versa_3.jpg'),

-- Cameras
(6, 3, 'Canon EOS R5', 'canon-eos-r5', 'High-resolution mirrorless camera for professionals', 3500.00, 'canoneosr5', 10, 'https://example.com/canon_eos_r5.jpg'),
(6, 3, 'Sony Alpha A7 III', 'sony-alpha-a7-iii', 'Full-frame mirrorless camera with excellent low-light performance', 2000.00, 'sonyalphaa7iii', 15, 'https://example.com/sony_alpha_a7iii.jpg'),
(6, 4, 'Nikon Z7 II', 'nikon-z7-ii', 'Mirrorless camera with high-resolution sensor and 4K video', 2800.00, 'nikonz7ii', 12, 'https://example.com/nikon_z7_ii.jpg'),

-- Televisions
(7, 1, 'Samsung QLED QN90A', 'samsung-qled-qn90a', 'Immersive 4K QLED TV with Quantum HDR', 2500.00, 'samsungqn90a', 8, 'https://example.com/samsung_qled_qn90a.jpg'),

-- Others
(8, 2, 'Samsung 32" Curved Monitor', 'samsung-32-curved-monitor', 'Immersive gaming and viewing experience', 400.00, 'sammon32curved', 20, 'https://example.com/samsung_curved_monitor.jpg'),
(8, 3, 'Logitech G Pro Mechanical Keyboard', 'logitech-g-pro-mechanical-keyboard', 'Designed for pro-grade performance', 150.00, 'logitechgprokb', 40, 'https://example.com/logitech_g_pro_keyboard.jpg'),
(8, 4, 'WD Elements 2TB External Hard Drive', 'wd-elements-2tb-external-hard-drive', 'High-capacity storage for your files', 80.00, 'wdele2tb', 50, 'https://example.com/wd_elements_2tb.jpg');


-- 
--      order 
-- 
-- Insert data into order table 
insert into "order" (customer_id, address_id, total_amount)
values
(1, 1, 2000),    -- Order 1
(1, 2, 1350),    -- Order 2
(2, 3, 1200),    -- Order 3
(3, 5, 6800),    -- Order 4
(5, 7, 1000),    -- Order 5
(8, 10, 5150),   -- Order 6
(4, 6, 1200);    -- Order 7


-- 
--      order_product 
-- 
-- Insert data into order_product table
-- Order 1 with 2 products
insert into order_product (order_id, product_id, quantity, price)
values
(1, 1, 1, 1500),   -- Dell XPS 13
(1, 7, 2, 500);    -- AirPods Pro

-- Order 2 with 2 products
insert into order_product (order_id, product_id, quantity, price)
values
(2, 11, 1, 900),   -- Microsoft Surface Pro 7
(2, 20, 3, 450);   -- Apple Watch Series 7

-- Order 3 with 1 product
insert into order_product (order_id, product_id, quantity, price)
values
(3, 4, 1, 1200);  -- iPhone 13 Pro

-- Order 4 with 3 products
insert into order_product (order_id, product_id, quantity, price)
values
(4, 9, 1, 800),     -- iPad Air
(4, 17, 2, 5600),   -- Nikon Z7 II
(4, 21, 5, 400);    -- WD Elements 2TB External Hard Drive

-- Order 5 with 1 product
insert into order_product (order_id, product_id, quantity, price)
values
(5, 6, 1, 1000);    -- Google Pixel 6 Pro

-- Order 6 with 2 products
insert into order_product (order_id, product_id, quantity, price)
values
(6, 20, 1, 150),    -- Logitech G Pro Mechanical Keyboard
(6, 18, 2, 5000);   -- Samsung QLED QN90A

-- Order 7 with 3 products
insert into order_product (order_id, product_id, quantity, price)
values
(7, 10, 1, 700),   -- Samsung Galaxy Tab S7
(7, 14, 1, 200),   -- Fitbit Versa 3
(7, 3, 1, 300);    -- HP Spectre x360


-- 
--      review 
-- 
-- Insert data into review table
insert into review (order_id, customer_id, product_id, rating, comment)
values
-- Order 1 - Dell XPS 13
(1, 1, 1, 5, 'Excellent laptop, very satisfied with the purchase'),
-- Order 1 - AirPods Pro
(1, 1, 7, 4, 'Good sound quality but could have better battery life'),
-- Order 2 - Microsoft Surface Pro 7
(2, 1, 11, 1, 'Disappointed, too heavy to carry around'),
-- Order 2 - Apple Watch Series 7
(2, 1, 20, 5, 'Love the design and features, very stylish'),
-- Order 3 - iPhone 13 Pro
(3, 2, 4, 5, 'Amazing phone, the camera quality is outstanding'),
-- Order 4 - iPad Air
(4, 3, 9, 5, 'Sleek design, powerful performance, highly recommended'),
-- Order 4 - Nikon Z7 II
(4, 3, 17, 4, 'Professional-grade camera, but a bit pricey'),
-- Order 4 - WD Elements 2TB External Hard Drive
(4, 3, 21, 5, 'Excellent storage solution, works flawlessly'),
-- Order 6 - Logitech G Pro Mechanical Keyboard
(6, 8, 20, 5, 'Perfect for gaming, very responsive and durable'),
-- Order 7 - Samsung Galaxy Tab S7
(7, 4, 10, 4, 'Fast tablet with beautiful display, recommended for productivity'),
-- Order 7 - Fitbit Versa 3
(7, 4, 14, 3, 'Decent fitness tracker, could use more features');


