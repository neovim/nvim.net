if exists('g:loaded_dotnet_provider')
  finish
endif
let g:loaded_dotnet_provider = 1
call provider#dotnet#Register()
