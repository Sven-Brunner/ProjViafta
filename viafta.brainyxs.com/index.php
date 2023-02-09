<?php

require "api.php";

$api = new Api();

$results = $api->getTops();
echo json_encode($results);
?>