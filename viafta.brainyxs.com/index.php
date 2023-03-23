<?php

require "api.php";
$api = new Api();

if ($_SERVER['REQUEST_METHOD'] === "GET") {
    $username = $_GET["user"];
    $from = $_GET["from"];
    $to = $_GET["to"];
    $results = $api->getTops($from, $to, $username);
    echo json_encode($results);
} else if ($_SERVER['REQUEST_METHOD'] === "POST") {
    $valid = $api->validateRequest();
    if ($valid) {
        $body = file_get_contents('php://input');
        $json = json_decode($body);
        $distance = $json->distance;
        $maxacceleration = $json->max_acceleration;
        $athlete = $json->athlete;
        $api->post($distance, $maxacceleration, $athlete);
        http_response_code(201);
    } else {
        http_response_code(418);
    }
} else {
    http_response_code(405);
}
?>