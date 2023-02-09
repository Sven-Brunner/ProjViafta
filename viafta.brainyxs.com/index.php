<?php

require "api.php";
if ($_SERVER['REQUEST_METHOD'] === "GET") {
    $api = new Api();
    $username = $_GET["user"];
    $from = $_GET["from"];
    $to = $_GET["to"];
    $results = $api->getTops($from, $to, $username);
    echo json_encode($results);
}
?>