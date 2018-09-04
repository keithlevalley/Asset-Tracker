# This is example code, not a true script

# This is just here to prevent script from running
return

# Formats for use with JSON
$params = '{
    "SerialNumber":  "TEST01",
    "Name":  "TEST",
    "Model":  "TEST",
    "MACAddress":  "TEST",
    "IpAddress":  "TEST"
}'

# Used for Asset Dump, input correct URL
Invoke-RestMethod -Uri "URL" -UseDefaultCredentials

# Formats for use with JSON
$params = '{
    "id":  "TEST1", "TEST2", "TEST3"
}'

$params = "TEST1", "TEST2", "TEST3" | ConvertTo-Json

$params = $params | ConvertTo-Json
$params

# Used for Deleting a list, input correct URL
Invoke-RestMethod -Uri "URL" -Method Post -Body $params -UseDefaultCredentials -ContentType "application/json"

<#
.Synopsis
   Short description
.DESCRIPTION
   Long description
.EXAMPLE
   Example of how to use this cmdlet
.EXAMPLE
   Another example of how to use this cmdlet
#>
function CheckIn-Asset
{
    $SSN = (Get-WmiObject -Class win32_Bios).SerialNumber.ToString()
    $Name = $env:COMPUTERNAME
    $Model = (Get-WmiObject -Class win32_ComputerSystem).SystemFamily.ToString()
    $MAC = (Get-NetAdapter | where-object {$_.PhysicalMediaType -eq "802.3" -and $_.Status -eq "Up"}).MacAddress.ToString()
    $IP = (Get-NetIPAddress | Where-Object {$_.PrefixOrigin -eq "Dhcp"}).IPAddress.ToString()

    $params = "{
    `"SerialNumber`":  `"$SSN`",
    `"Name`":  `"$Name`",
    `"Model`":  `"$Model`",
    `"MACAddress`":  `"$MAC`",
    `"IpAddress`":  `"$IP`"
}"

Invoke-RestMethod -Uri "URL" -Method Post -Body $params -UseDefaultCredentials -ContentType "application/json"

}

Get-Assets | where {$_.Model -eq "TEST"}
Get-Assets | where {$_.Storage}

Delete-Assets -id TEST111, TEST112

$assets = Get-Assets | where {$_.Storage}

$assets = Get-Assets

$assets | where {$_.Model -eq "TEST"}

Delete-Assets -id TEST111, TEST112

$output = @()

foreach ($item in $assets)
{
    $output += $item.SerialNumber
}

Delete-Assets -id $output