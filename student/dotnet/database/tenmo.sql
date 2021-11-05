USE master
GO

IF DB_ID('tenmo') IS NOT NULL
BEGIN
	ALTER DATABASE tenmo SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
	DROP DATABASE tenmo;
END

CREATE DATABASE tenmo
GO

USE tenmo
GO

CREATE TABLE transfer_types (
	transfer_type_id int IDENTITY(1,1) NOT NULL,
	transfer_type_desc varchar(10) NOT NULL,
	CONSTRAINT PK_transfer_types PRIMARY KEY (transfer_type_id)
)

CREATE TABLE transfer_statuses (
	transfer_status_id int IDENTITY(1,1) NOT NULL,
	transfer_status_desc varchar(10) NOT NULL,
	CONSTRAINT PK_transfer_statuses PRIMARY KEY (transfer_status_id)
)

CREATE TABLE users (
	user_id int IDENTITY(1001,1) NOT NULL,
	username varchar(50) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	CONSTRAINT PK_user PRIMARY KEY (user_id),
	CONSTRAINT UQ_username UNIQUE (username)
)

CREATE TABLE accounts (
	account_id int IDENTITY(2001,1) NOT NULL,
	user_id int NOT NULL,
	balance decimal(13, 2) NOT NULL,
	CONSTRAINT PK_accounts PRIMARY KEY (account_id),
	CONSTRAINT FK_accounts_user FOREIGN KEY (user_id) REFERENCES users (user_id)
)

CREATE TABLE transfers (
	transfer_id int IDENTITY(3001,1) NOT NULL,
	transfer_type_id int NOT NULL,
	transfer_status_id int NOT NULL,
	account_from int NOT NULL,
	account_to int NOT NULL,
	amount decimal(13, 2) NOT NULL,
	CONSTRAINT PK_transfers PRIMARY KEY (transfer_id),
	CONSTRAINT FK_transfers_accounts_from FOREIGN KEY (account_from) REFERENCES accounts (account_id),
	CONSTRAINT FK_transfers_accounts_to FOREIGN KEY (account_to) REFERENCES accounts (account_id),
	CONSTRAINT FK_transfers_transfer_statuses FOREIGN KEY (transfer_status_id) REFERENCES transfer_statuses (transfer_status_id),
	CONSTRAINT FK_transfers_transfer_types FOREIGN KEY (transfer_type_id) REFERENCES transfer_types (transfer_type_id),
	CONSTRAINT CK_transfers_not_same_account CHECK  ((account_from<>account_to)),
	CONSTRAINT CK_transfers_amount_gt_0 CHECK ((amount>0))
)


INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Pending');
INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Approved');
INSERT INTO transfer_statuses (transfer_status_desc) VALUES ('Rejected');

INSERT INTO transfer_types (transfer_type_desc) VALUES ('Request');
INSERT INTO transfer_types (transfer_type_desc) VALUES ('Send');


GET * FROM users

SELECT transfer_id,transfer_type_id,transfer_status_id,account_from,account_to,amount
FROM transfers
WHERE account_from = {id} OR  account_to = {id}

INSERT INTO transfers (transfer_type_id,transfer_status_id,account_from,account_to,amount)
OUTPUT inserted.transfer_id
VALUES ({transfer_type_id},{transfer_status_id},{account_from},{account_to},{amount})

INSERT INTO transfers (transfer_type_id, transfer_status_id,account_from,account_to,amount)
VALUES (1,1,2001,2002,1),(2,2,2002,2001,2)

SELECT * FROM accounts
SELECT * FROM transfers

SELECT * FROM transfer_statuses

SELECT * FROM accounts


SELECT account_id, user_id, balance, 2 as a
from accounts 
where account_id = 2001

SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 1 as Sender,username as name
FROM transfers 
LEFT JOIN accounts on account_from = account_id
LEFT JOIN users on users.user_id = accounts.user_id
WHERE account_from = {accountId}

UNION

SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 0 as Sender,username as name
FROM transfers
LEFT JOIN accounts on account_to = account_id
LEFT JOIN users on users.user_id = accounts.user_id
WHERE account_to = {accountId}


SELECT transfer_id,transfer_type_id,transfer_status_id,account_from,account_to,amount,1 as Sender,usersfrom.username as name_from,usersto.username as name_to
FROM transfers
LEFT JOIN accounts as fromaccount on account_from = fromaccount.account_id
LEFT JOIN accounts as toaccount on account_to = toaccount.account_id

LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id

WHERE transfer_id = 3003


SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 1 as Sender,usersfrom.username as name_from,usersto.username as name_to
FROM transfers 

LEFT JOIN accounts as fromaccount on account_from = account_id
LEFT JOIN accounts as toaccount on toaccount.account_id = account_to

LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id

WHERE account_from = 2001

UNION

SELECT transfer_id, transfer_type_id,  transfer_status_id, account_from, account_to, amount, 0 as Sender,usersfrom.username as name_from,usersto.username as name_to
FROM transfers 

LEFT JOIN accounts as fromaccount on account_from = account_id
LEFT JOIN accounts as toaccount on toaccount.account_id = account_to

LEFT JOIN users as usersfrom on usersfrom.user_id = fromaccount.user_id
LEFT JOIN users as usersto on usersto.user_id = toaccount.user_id
WHERE account_to = 2001