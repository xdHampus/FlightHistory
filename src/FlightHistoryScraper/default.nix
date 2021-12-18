{ 
	pkgs, lib, buildDotnetModule, dotnetCorePackages, 
	sqlite 
}:

let
  fhCore = pkgs.callPackage ../FlightHistoryCore/default.nix {};
in buildDotnetModule rec {
  pname = "FlightHistoryScraper";
  version = "0.1";
  src = ./.;
  projectFile = "FlightHistoryScraper.csproj";
  
  nugetDeps = ./deps.nix; # File generated with `nuget-to-nix path/to/src > deps.nix`.
  projectReferences = [ fhCore ]; # `referencedProject` must contain `nupkg` in the folder structure.
 
  dotnet-sdk = dotnetCorePackages.sdk_5_0;
  dotnet-runtime = dotnetCorePackages.runtime_5_0;
  runtimeDeps = [ 
  		sqlite
  ]; 

  
  dotnetFlags = [ 
  	#"--runtime 64-linux" 
  	#"/t:src\FlightHistoryScraper"
  ];
  executables = [ "FlightHistoryScraper" ]; # This wraps "$out/lib/$pname/foo" to `$out/bin/foo`.
  #executables = []; # Don't install any executables.

}

