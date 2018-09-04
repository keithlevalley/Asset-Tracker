<#
.Synopsis
   This will return all Assets from the Database
.DESCRIPTION
   Long description
.EXAMPLE
   Returns all Assets

   Get-Assets
.EXAMPLE
   Outputs assets into a new grid view window

   Get-Assets | Out-GridView
.EXAMPLE
   Returns all Assets with a Model of "TEST"

   Get-Assets | where {$_.Model -eq "TEST"}
.EXAMPLE
   Returns all Assets in storage

   Get-Assets | where {$_.Storage}
.EXAMPLE
   Saves all Assets into variable $assets

   $assets = Get-Assets

.EXAMPLE
   Saves assets into a file called assets.csv

   Get-Assets | Export-Csv -Path assets.csv

#>
function Get-Assets
{
    $assets = Invoke-RestMethod -Uri "URL" -UseDefaultCredentials
    $assets = $assets | Select-Object -Property Name, SerialNumber, Model, CreationDate, LastCheckin, Managed, Storage, MACAddress, IpAddress, comments
    return $assets
}

<#
.Synopsis
   This function will delete assets using Serial Number from the Asset list, number of assets deleted is returned
.DESCRIPTION
   Long description
.EXAMPLE
   Deletes asset with serial number TEST111 from Assets

   Delete-Assets -id TEST111
.EXAMPLE
   Deletes assets with serial numbers TEST111 and TEST112 from Assets

   Delete-Assets -id TEST111, TEST112
.EXAMPLE
   Deletes array of asset objects with variable $assets from Assets

   Delete-Assets -id ($assets | select -ExpandProperty SerialNumber)
.EXAMPLE
   Deletes array of strings with variable $assets from Assets

   Delete-Assets -id $assets
#>
function Delete-Assets
{
    Param
    (
        # Param1 help description
        [Parameter(Mandatory=$true,
                   ValueFromPipelineByPropertyName=$true,
                   Position=0)]
        [string[]]
        $id
    )

    if ($id.Count -eq 1)
    {
        $id = $id, ""
    }
        
    $params = $id | ConvertTo-Json

    Invoke-RestMethod -Uri "URL" -Method Post -Body $params -UseDefaultCredentials -ContentType "application/json"
}

Delete-Assets -id ($assets | select -ExpandProperty SerialNumber)