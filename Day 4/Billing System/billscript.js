// Product prices
const products = {
    Pen: 10,
    Notebook: 50,
    Pencil: 5,
    Bag: 500
};


// Selected products
let cart = {};


// Add product
function addProduct(productName) {

    if (cart[productName]) {
        cart[productName]++;
    } else {
        cart[productName] = 1;
    }

    displayBill();
}


// Display bill
function displayBill() {

    const billItems = document.getElementById("billItems");

    billItems.innerHTML = "";

    let total = 0;


    for (let productName in cart) {

        const quantity = cart[productName];

        const price = products[productName];

        const itemTotal = quantity * price;

        total += itemTotal;


        billItems.innerHTML += `
            <div class="bill-item">

                <span>
                    ${productName} × ${quantity}
                </span>

                <span>
                    ₹${itemTotal}
                </span>

            </div>
        `;
    }


    document.getElementById("total").textContent = total;
}