{
  description = "Flight history analysis program";

  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/master";
    utils.url = "github:numtide/flake-utils";
    utils.inputs.nixpkgs.follows = "nixpkgs";
  };

  outputs = { self, nixpkgs, ... }@inputs: inputs.utils.lib.eachSystem [
    "x86_64-linux" "i686-linux" "aarch64-linux" "x86_64-darwin"
  ] (system: let pkgs = import nixpkgs {
                   inherit system;
                   overlays = [];
                   # config.allowUnfree = true;
                 };
             in {
				devShell = pkgs.mkShell rec {
                 # Update the name to something that suites your project.
	                 name = "flighthistory";

	                 packages = with pkgs; [
	                 		nuget-to-nix
	                 		dotnet-sdk_5
					 		dotnetCorePackages.sdk_3_1
						  	dotnetCorePackages.runtime_5_0
						  	sqlite		
	                 ];
                 
				};
  				packages.x86_64-linux.core = pkgs.callPackage ./src/FlightHistoryCore/default.nix {};
  				packages.x86_64-linux.scraper = pkgs.callPackage ./src/FlightHistoryScraper/default.nix {};
  				#packages."x86_64-linux"."visualizer" = derivation;
				
				defaultPackage = pkgs.callPackage ./src/FlightHistoryScraper/default.nix {};
             });
}
