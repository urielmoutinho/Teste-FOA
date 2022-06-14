# TESTE FOA

## Erros encontrados

1-> 

public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            //services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }

	1.1-> 
    
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
        }
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

2-> Esse tipo de erro faz com que o conteúdo do body dentro do main fique invisível para o usuário que esta navegando na pagina.
      <div class="container">
            <main hidden role="main" class="pb-3">
                @RenderBody()
            </main>
        </div>

	2.1-> para corrigilo basta somente retirar a tag de CSS hidden que esta dando essa estilização invisível para o main.
    
         <div class="container">
                <main role="main" class="pb-3">
                @RenderBody()
                </main>
            </div>

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

3-> Este é um erro de junção na função de banco de dados, com esse tipo de erro os dados vindos das duas tabelas são iguais, assim duplicando os dados e cadastrando o mesmo livro mais duas vezes.

 public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookViewModel.FromSqlRaw(@"
                select B.Id,
                    B.AuthorId,
                    A.Name as AuthorName,
                    B.ISBN,
                    B.PublishDate,
                    B.Publisher,
                    B.Title
                from Book B
                    inner join Author A on A.Id = A.Id");
            return View(await applicationDbContext.ToListAsync());
        }

	3.1-> Neste caso a forma correta da solução do erro é com a comparação ao b.AuthorId onde retorna somente uma vez ao usuário o livro cadastrado.

    public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BookViewModel.FromSqlRaw(@"
                select B.Id,
                    B.AuthorId,
                    A.Name as AuthorName,
                    B.ISBN,
                    B.PublishDate,
                    B.Publisher,
                    B.Title
                from Book B
                    inner join Author A on A.Id = B.AuthorId");
            return View(await applicationDbContext.ToListAsync());
        }

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------


4-> Erro onde todas as strings se encontram privadas, assim fazendo com que assim que o usuário cadastre não seja possivel o retorno e a visualização da mesma.

    public string ISBN { get; private set; }
    public string Publisher { get; private set; }
    public string Title { get; private set; }

	4.1-> A solução é bem simples, com a retirada do atributo de private o usuário retorna a visualizar as informações armazenadas em Titulo, ISBN e Publisher.
        public string ISBN { get; set; }

        public string Publisher { get; set; }

        public string Title { get; set; }

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------


5-> A falta do Title implica na não criação do titulo quando inserido na aba de criação de livros.
    
    public async Task<IActionResult> Create([Bind("Id,AuthorId,Publisher,ISBN,PublishDate")] Book book)

	5.1->Erro é facilmente corrigido com a inserção do Title dentro do Bind.
        
        public async Task<IActionResult> Create([Bind("Id,Title,AuthorId,Publisher,ISBN,PublishDate")] Book book)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

6->A falta do Title implica na não alteração do titulo quando inserido na aba de alteração de livros.

    public async Task<IActionResult> Edit(Guid id, [Bind("Id,AuthorId,Publisher,ISBN,PublishDate")] Book book)

	6.1->Erro é facilmente corrigido com a inserção do Title dentro do Bind.
    
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,AuthorId,Publisher,ISBN,PublishDate")] Book book)

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

7. Erro na remoção do livro, que deve acontecer antes do salvamento das informação.

    public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _context.Book.FindAsync(id);
            await _context.SaveChangesAsync();
            _context.Book.Remove(book);
            return RedirectToAction(nameof(Index));
        }

	7.1->Erro pode ser resolvido com a inversão de ordem das funções assim tento exito em excluir o livro desejado.
    
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

## Itens Bonus Completados
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Ordenar os resultados dos autores por nome.

    public async Task<IActionResult> Index(string sortOrder, string searchString)
        { 
            ViewData["CurrentFilter"] = searchString;
            var livros = from s in _context.BookViewModel.FromSqlRaw(@"
                select B.Id,
                    B.AuthorId,
                    A.Name as AuthorName,
                    B.ISBN,
                    B.PublishDate,
                    B.Publisher,
                    B.Title
                from Book B
                        inner join Author A on A.Id = B.AuthorId")
                select s;
                if (!String.IsNullOrEmpty(searchString))
                    {
                        livros = livros.Where(s => s.Title.Contains(searchString));
                    }
                    switch (sortOrder)
                    {
                        case "name_desc":
                            livros = livros.OrderByDescending(s => s.Title);
                            break;
                        default:
                            livros = livros.OrderBy(s => s.Title);
                            break;
                    }
            return View(await livros.AsNoTracking().ToListAsync());


        }
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Ordenar os resultados dos livros por título.

    public async Task<IActionResult> Index(string sortOrder, string searchString)
            {       
                ViewData["CurrentFilter"] = searchString;
                var Autores = from s in _context.Author
                    select s;
                    if (!String.IsNullOrEmpty(searchString))
                        {
                            Autores = Autores.Where(s => s.Name.Contains(searchString));
                        }
                        switch (sortOrder)
                        {
                            case "name_desc":
                                Autores = Autores.OrderByDescending(s => s.Name);
                                break;
                            default:
                                Autores = Autores.OrderBy(s => s.Name);
                                break;
                        }
                return View(await Autores.AsNoTracking().ToListAsync());
        
            }
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Adicionar pesquisa por nome de autor.

    <form asp-action="Index" method="get">
        <div class="form-actions no-color">
            <p>
                Find by name: <input type="text" name="SearchString" value=""/>
                <input type="submit" value="Search" class="btn btn-default" /> |
                <a asp-action="Index">Back to Full List</a>
            </p>
        </div>
    </form>
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
Adicionar pesquisa por título de livro.

    <form asp-action="Index" method="get">
        <div class="form-actions no-color">
            <p>
                Find by title: <input type="text" name="SearchString" value=""/>
                <input type="submit" value="Search" class="btn btn-default" /> |
                <a asp-action="Index">Back to Full List</a>
            </p>
        </div>
    </form>
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------