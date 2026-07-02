const CACHE_NAME = "reyes-cache-v2";

const urlsToCache = [
    "/css/site.css",
    "/css/tienda.css",
    "/js/site.js",
    "/images/logo.png",
    "/images/banner.png",
    "/favicon.ico"
];

self.addEventListener("install", event => {
    event.waitUntil(
        caches.open(CACHE_NAME)
            .then(cache => cache.addAll(urlsToCache))
    );

    self.skipWaiting();
});

self.addEventListener("activate", event => {

    event.waitUntil(

        caches.keys().then(keys =>

            Promise.all(
                keys.map(key => {

                    if (key !== CACHE_NAME)
                        return caches.delete(key);

                })
            )

        )

    );

    self.clients.claim();
});

self.addEventListener("fetch", event => {

    if (event.request.mode === "navigate") {

        event.respondWith(fetch(event.request));

        return;
    }

    event.respondWith(

        caches.match(event.request)
            .then(response => response || fetch(event.request))

    );

});