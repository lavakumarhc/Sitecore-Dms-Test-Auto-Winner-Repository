# Sitecore-Dms-Test-Auto-Winner-Repository
Sitecore-Dms-Test-Auto-Winner

Sitecore Dms 8.1 Content/MV Test Automatic Winner selection Determine the Test Winner Automatically based on the Site Statistics like, Bounce rate, Time spend on Page etc This is an attempt to extend Sitecore's analytics capabilities to better support Sitecore 8.1 DMS features like Content testing and MVT.Sitecore Automatically select the winner based on engagement values and score calculated by sitecore and Running the job to end test ruleset. all of the configuration is done through Sitecore's Rule Engine. Customize rules set to override default rule set by Sitecore and the configuration as below in the ContentTesting config.

  <!-- CONTENT TESTING - END TEST RULESET -->
       The path to the ruleset that determines whether a test can finish automatically.
       Default value: /sitecore/system/Settings/Content Testing/End Test Behavior
We can add Conditions and Action to the rules engine based the requirements. In this Project Rules action added to select the Winner based on the Less bounce rate recorded by the text experience.
