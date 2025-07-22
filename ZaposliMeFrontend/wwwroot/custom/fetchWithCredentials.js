window.fetchWithCredentials = {
    send: async function (url, options) {
        // Prepare fetch init object
        const init = {
            method: options.method,
            headers: options.headers || {},
            credentials: options.credentials || "include"
        };

        // Handle body
        if (options.body !== undefined && options.body !== null) {
            // If body is base64 string, decode it to Uint8Array for binary content
            if (typeof options.body === "string" && /^[A-Za-z0-9+/=]+$/.test(options.body) && options.method !== "GET" && options.method !== "HEAD") {
                // Assume base64 for non-GET/HEAD
                init.body = Uint8Array.from(atob(options.body), c => c.charCodeAt(0));
            } else {
                init.body = options.body;
            }
        }

        const response = await fetch(url, init);

        const headers = {};
        response.headers.forEach((v, k) => {
            headers[k] = v;
        });

        // Get content type
        const contentType = response.headers.get("content-type") || "";

        // Try to get body as text or binary
        let body = "";
        let isBase64 = false;
        if (contentType.startsWith("application/json") || contentType.startsWith("text/")) {
            body = await response.text();
        } else {
            // Binary or unknown content, convert to base64 string
            const buffer = await response.arrayBuffer();
            const bytes = new Uint8Array(buffer);
            body = btoa(String.fromCharCode(...bytes));
            isBase64 = true;
        }

        return {
            status: response.status,
            statusText: response.statusText,
            headers: headers,
            body: body,
            contentType: contentType,
            isBase64: isBase64
        };
    }
};
