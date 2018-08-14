let s:plugin_host_path =
  \ resolve(expand('<sfile>:p:h').'/../../src/NvimPluginHost')

function! provider#dotnet#Require(host) abort
  return provider#Poll(['dotnet', 'run', '--project', s:plugin_host_path],
    \ a:host.orig_name, '$NVIM_DOTNET_LOG_FILE')
endfunction

function! provider#dotnet#Register()
  " The glob pattern '../../*.sln' will be appended to 'rplugin/dotnet/' and
  " used to find dotnet solution files in the root of the plugin directories.
  call remote#host#Register('dotnet', '../../*.sln',
    \ function('provider#dotnet#Require'))
endfunction
