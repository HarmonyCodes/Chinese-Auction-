import React, { useContext } from "react";
import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";
import { Message } from "primereact/message";
// נניח שיש CartContext שמספק cart, removeFromCart, total
import { CartContext } from "../context/CartContext";

const Cart = () => {
  const { cart, removeFromCart, total } = useContext(CartContext);

  const actionBody = (rowData) => (
    <Button icon="pi pi-trash" className="p-button-danger" onClick={() => removeFromCart(rowData.id)} />
  );

  return (
    <div className="flex justify-content-center align-items-center" style={{ minHeight: "80vh" }}>
      <Card title="סל הקניות שלי" style={{ width: "600px" }}>
        {cart.length === 0 ? (
          <Message severity="info" text="הסל ריק. הוסף מתנות כדי להמשיך." />
        ) : (
          <>
            <DataTable value={cart} responsiveLayout="scroll">
              <Column field="name" header="שם מתנה" />
              <Column field="price" header="מחיר" body={row => row.price + " ₪"} />
              <Column field="quantity" header="כמות" />
              <Column body={actionBody} header="הסר" style={{ width: "80px" }} />
            </DataTable>
            <div className="flex justify-content-end mt-4">
              <strong>סה"כ: {total} ₪</strong>
            </div>
            <Button label="לתשלום" icon="pi pi-credit-card" className="mt-3 p-button-success" />
          </>
        )}
      </Card>
    </div>
  );
};

export default Cart;
