{
    "info": {
        "name": "E-Commerce API",
            "description": "Complete REST API collection for E-Commerce System with JWT Authentication",
                "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
    },
    "variable": [
        {
            "key": "baseUrl",
            "value": "https://localhost:7001/api",
            "type": "string"
        },
        {
            "key": "token",
            "value": "",
            "type": "string"
        }
    ],
        "auth": {
        "type": "bearer",
            "bearer": [
                {
                    "key": "token",
                    "value": "{{token}}",
                    "type": "string"
                }
            ]
    },
    "item": [
        {
            "name": "Authentication",
            "item": [
                {
                    "name": "Register Customer",
                    "event": [
                        {
                            "listen": "test",
                            "script": {
                                "exec": [
                                    "if (pm.response.code === 200) {",
                                    "    var jsonData = pm.response.json();",
                                    "    pm.collectionVariables.set('token', jsonData.token);",
                                    "}"
                                ]
                            }
                        }
                    ],
                    "request": {
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"email\": \"newcustomer@test.com\",\n  \"password\": \"Customer@123\",\n  \"confirmPassword\": \"Customer@123\",\n  \"firstName\": \"John\",\n  \"lastName\": \"Doe\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/auth/register",
                            "host": ["{{baseUrl}}"],
                            "path": ["auth", "register"]
                        }
                    }
                },
                {
                    "name": "Login Admin",
                    "event": [
                        {
                            "listen": "test",
                            "script": {
                                "exec": [
                                    "if (pm.response.code === 200) {",
                                    "    var jsonData = pm.response.json();",
                                    "    pm.collectionVariables.set('token', jsonData.token);",
                                    "}"
                                ]
                            }
                        }
                    ],
                    "request": {
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"email\": \"admin@ecommerce.com\",\n  \"password\": \"Admin@123\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/auth/login",
                            "host": ["{{baseUrl}}"],
                            "path": ["auth", "login"]
                        }
                    }
                },
                {
                    "name": "Login Customer",
                    "event": [
                        {
                            "listen": "test",
                            "script": {
                                "exec": [
                                    "if (pm.response.code === 200) {",
                                    "    var jsonData = pm.response.json();",
                                    "    pm.collectionVariables.set('token', jsonData.token);",
                                    "}"
                                ]
                            }
                        }
                    ],
                    "request": {
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"email\": \"customer@ecommerce.com\",\n  \"password\": \"Customer@123\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/auth/login",
                            "host": ["{{baseUrl}}"],
                            "path": ["auth", "login"]
                        }
                    }
                },
                {
                    "name": "Refresh Token",
                    "request": {
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"accessToken\": \"{{token}}\",\n  \"refreshToken\": \"your-refresh-token-here\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/auth/refresh",
                            "host": ["{{baseUrl}}"],
                            "path": ["auth", "refresh"]
                        }
                    }
                },
                {
                    "name": "Logout",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/auth/logout",
                            "host": ["{{baseUrl}}"],
                            "path": ["auth", "logout"]
                        }
                    }
                }
            ]
        },
        {
            "name": "Products",
            "item": [
                {
                    "name": "Get All Products",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products",
                            "host": ["{{baseUrl}}"],
                            "path": ["products"]
                        }
                    }
                },
                {
                    "name": "Get Product By ID",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "1"]
                        }
                    }
                },
                {
                    "name": "Search Products",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products/search?q=laptop",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "search"],
                            "query": [
                                {
                                    "key": "q",
                                    "value": "laptop"
                                }
                            ]
                        }
                    }
                },
                {
                    "name": "Get Products By Category",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products/category/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "category", "1"]
                        }
                    }
                },
                {
                    "name": "Get Low Stock Products",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products/low-stock",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "low-stock"]
                        }
                    }
                },
                {
                    "name": "Create Product",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"name\": \"Wireless Keyboard\",\n  \"description\": \"Mechanical wireless keyboard with RGB\",\n  \"price\": 89.99,\n  \"stockQuantity\": 50,\n  \"categoryId\": 1,\n  \"sku\": \"ELEC-KEY-001\",\n  \"imageUrl\": \"https://via.placeholder.com/300x300?text=Keyboard\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/products",
                            "host": ["{{baseUrl}}"],
                            "path": ["products"]
                        }
                    }
                },
                {
                    "name": "Update Product",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "PUT",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"name\": \"Wireless Keyboard Updated\",\n  \"description\": \"Updated description\",\n  \"price\": 79.99,\n  \"stockQuantity\": 45,\n  \"categoryId\": 1,\n  \"sku\": \"ELEC-KEY-001\",\n  \"imageUrl\": \"https://via.placeholder.com/300x300?text=Keyboard\",\n  \"isActive\": true\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/products/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "1"]
                        }
                    }
                },
                {
                    "name": "Delete Product",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "DELETE",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/products/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["products", "1"]
                        }
                    }
                }
            ]
        },
        {
            "name": "Categories",
            "item": [
                {
                    "name": "Get All Categories",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/categories",
                            "host": ["{{baseUrl}}"],
                            "path": ["categories"]
                        }
                    }
                },
                {
                    "name": "Get Category By ID",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/categories/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["categories", "1"]
                        }
                    }
                },
                {
                    "name": "Get Category Products",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/categories/1/products",
                            "host": ["{{baseUrl}}"],
                            "path": ["categories", "1", "products"]
                        }
                    }
                },
                {
                    "name": "Create Category",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"name\": \"Gaming\",\n  \"description\": \"Gaming accessories and equipment\",\n  \"isActive\": true\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/categories",
                            "host": ["{{baseUrl}}"],
                            "path": ["categories"]
                        }
                    }
                }
            ]
        },
        {
            "name": "Cart",
            "item": [
                {
                    "name": "Get My Cart",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/cart",
                            "host": ["{{baseUrl}}"],
                            "path": ["cart"]
                        }
                    }
                },
                {
                    "name": "Add To Cart",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"productId\": 1,\n  \"quantity\": 2\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/cart/items",
                            "host": ["{{baseUrl}}"],
                            "path": ["cart", "items"]
                        }
                    }
                },
                {
                    "name": "Update Cart Item",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "PUT",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"quantity\": 3\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/cart/items/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["cart", "items", "1"]
                        }
                    }
                },
                {
                    "name": "Remove From Cart",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "DELETE",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/cart/items/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["cart", "items", "1"]
                        }
                    }
                },
                {
                    "name": "Clear Cart",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "DELETE",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/cart/clear",
                            "host": ["{{baseUrl}}"],
                            "path": ["cart", "clear"]
                        }
                    }
                }
            ]
        },
        {
            "name": "Orders",
            "item": [
                {
                    "name": "Get My Orders",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/orders",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders"]
                        }
                    }
                },
                {
                    "name": "Get All Orders (Admin)",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/orders/all",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders", "all"]
                        }
                    }
                },
                {
                    "name": "Get Order By ID",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/orders/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders", "1"]
                        }
                    }
                },
                {
                    "name": "Create Order",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"shippingAddress\": \"123 Main Street\",\n  \"city\": \"New York\",\n  \"postalCode\": \"10001\",\n  \"country\": \"USA\",\n  \"notes\": \"Please call before delivery\",\n  \"items\": [\n    {\n      \"productId\": 1,\n      \"quantity\": 2\n    },\n    {\n      \"productId\": 2,\n      \"quantity\": 1\n    }\n  ]\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/orders",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders"]
                        }
                    }
                },
                {
                    "name": "Cancel Order",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "PUT",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/orders/1/cancel",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders", "1", "cancel"]
                        }
                    }
                },
                {
                    "name": "Update Order Status (Admin)",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "PUT",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"status\": 2\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/orders/1/status",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders", "1", "status"]
                        }
                    }
                },
                {
                    "name": "Get Total Sales (Admin)",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/orders/sales",
                            "host": ["{{baseUrl}}"],
                            "path": ["orders", "sales"]
                        }
                    }
                }
            ]
        },
        {
            "name": "Reviews",
            "item": [
                {
                    "name": "Get Product Reviews",
                    "request": {
                        "method": "GET",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/reviews/products/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["reviews", "products", "1"]
                        }
                    }
                },
                {
                    "name": "Create Review",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "POST",
                        "header": [],
                        "body": {
                            "mode": "raw",
                            "raw": "{\n  \"rating\": 5,\n  \"title\": \"Great product!\",\n  \"comment\": \"Excellent quality and fast shipping\"\n}",
                            "options": {
                                "raw": {
                                    "language": "json"
                                }
                            }
                        },
                        "url": {
                            "raw": "{{baseUrl}}/reviews/products/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["reviews", "products", "1"]
                        }
                    }
                },
                {
                    "name": "Delete Review",
                    "request": {
                        "auth": {
                            "type": "bearer",
                            "bearer": [
                                {
                                    "key": "token",
                                    "value": "{{token}}",
                                    "type": "string"
                                }
                            ]
                        },
                        "method": "DELETE",
                        "header": [],
                        "url": {
                            "raw": "{{baseUrl}}/reviews/1",
                            "host": ["{{baseUrl}}"],
                            "path": ["reviews", "1"]
                        }
                    }
                }
            ]
        }
    ]
}