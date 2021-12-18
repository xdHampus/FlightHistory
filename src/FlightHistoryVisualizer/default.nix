{ 
	lib, buildDotnetModule, dotnetCorePackages, 
	sqlite 
}:

buildDotnetModule rec {
  pname = "FlightHistory";
  version = "0.1";
  src = ./.;
  projectFile = "src/FlightHistory.sln";
  
  nugetDeps = ./deps.nix; # File generated with `nuget-to-nix path/to/src > deps.nix`.
 
  dotnet-sdk = dotnetCorePackages.sdk_3_1;
  dotnet-runtime = dotnetCorePackages.runtime_5_0;
  runtimeDeps = [ 
  		sqlite
  ]; 

  
  dotnetFlags = [ 
  	#"--runtime 64-linux" 
  	"/t:src\FlightHistoryScraper"
  ];
  executables = [ "foo" ]; # This wraps "$out/lib/$pname/foo" to `$out/bin/foo`.
  #executables = []; # Don't install any executables.

}

