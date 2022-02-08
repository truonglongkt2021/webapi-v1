pushd Repository\StreamFile.Repository
dotnet ef migrations add Init -v --context AppDbContext
dotnet ef database update -v --context AppDbContext
popd