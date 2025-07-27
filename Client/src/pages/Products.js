import React, { useEffect, useState } from "react";
import { Card } from "primereact/card";
import { DataTable } from "primereact/datatable";
import { Column } from "primereact/column";
import { Button } from "primereact/button";
import api from '../services/api';

const Products = () => {
  const [products, setProducts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const res = await api.get("/gifts");
        setProducts(res.data);
      } catch (err) {
        setError("שגיאה בטעינת המוצרים");
      } finally {
        setLoading(false);
      }
    };
    fetchProducts();
  }, []);

  return (
    <div className="flex justify-content-center align-items-center" style={{ minHeight: "80vh" }}>
      <Card title="רשימת מוצרים" style={{ width: "700px" }}>
        {error && <div style={{ color: 'red' }}>{error}</div>}
        <DataTable value={products} loading={loading} responsiveLayout="scroll">
          <Column field="name" header="שם מוצר" />
          <Column field="description" header="תיאור" />
          <Column field="price" header="מחיר" body={row => row.price + " ₪"} />
        </DataTable>
      </Card>
    </div>
  );
};

export default Products;
