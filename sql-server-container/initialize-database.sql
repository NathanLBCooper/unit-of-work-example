IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'mydatabasedb')
  BEGIN
    CREATE DATABASE mydatabasedb;
  END
