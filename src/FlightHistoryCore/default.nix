{ 
	lib, buildDotnetModule, dotnetCorePackages, 
	sqlite 
}:

buildDotnetModule rec {
  pname = "FlightHistoryCore";
  version = "0.1";
  src = ./.;
  projectFile = "FlightHistoryCore.csproj";
  
  nugetDeps = ./deps.nix; # File generated with `nuget-to-nix path/to/src > deps.nix`.
 
  dotnet-sdk = dotnetCorePackages.sdk_5_0;
  dotnet-runtime = dotnetCorePackages.runtime_5_0;
  runtimeDeps = [ 
  		sqlite
  ]; 

  packNupkg = true;
  #dotnetFlags = [ 
  #	#"--runtime 64-linux" 
  #	"/t:src\FlightHistoryScraper"
  #];
  
  #executables = [ "foo" ]; # This wraps "$out/lib/$pname/foo" to `$out/bin/foo`.
  executables = []; # Don't install any executables.

}

