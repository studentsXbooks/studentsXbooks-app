INSERT INTO [dbo].[AspNetUsers]
    ([Id], [UserName], [NormalizedUsername], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnabled], [AccessFailedCount], [Discriminator], [FirstName], [LastName])
VALUES
    (N'1234', N'TestUser', N'TESTUSER', N'test@wvup.edu', N'TEST@WVUP.EDU' , 1, N'AQAAAAEAACcQAAAAEIUEM4IbTgIex/XyTSMtqEFDIxiFU8uVjsfMIXcdsX6RAPd7xfQeo8VUTIgD90BLlA==', N'YMWST2SD23HET7HVSN266YTFJ44GOCIZ', N'c6445081-f8f5-441e-ae73-edfe7aa2776b', 0,0,1, 0, N'User', N'Test' , N'User')
