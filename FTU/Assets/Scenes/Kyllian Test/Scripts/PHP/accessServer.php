<?php
include "connection.php";

$user = $_GET["user"];
$pw = $_GET["password"];

$sql = "SELECT * FROM players WHERE username='$user' AND password='$pw'";
$req = $bdd->prepare($sql);

if($req->execute())
    {
        if($req->rowCount() == 1)
            {
                echo "200";
            }
        else
            {
                echo "500";
            }
    }
else
    {
        echo "500";   
    }


?>