import React, { useState, useContext } from "react";
import { AuthContext } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { Message } from "primereact/message";
import api from '../services/api';



const Login = () => {
  const { login } = useContext(AuthContext);
  const [form, setForm] = useState({ username: "", password: "" });
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const res = await api.post("/auth/login", form);
      if (res.data && res.data.token) {
        login(res.data.token);
        navigate("/");
      } else {
        setError("שגיאה בנתוני ההתחברות");
      }
    } catch (err) {
      setError(err.response?.data?.message || "שם משתמש או סיסמה שגויים");
    }
  };

  return (
    <div className="flex justify-content-center align-items-center" style={{ minHeight: "80vh" }}>
      <Card title="התחברות" style={{ width: "350px" , top: "20px" , marginLeft: "650px" }}>
        <form onSubmit={handleSubmit} className="p-fluid">
          <label htmlFor="username">שם משתמש</label>
          <InputText id="username" name="username" value={form.username} onChange={handleChange} autoFocus />
          <label htmlFor="password" className="mt-3">סיסמה</label>
          <Password id="password" name="password" value={form.password} onChange={handleChange} toggleMask feedback={false} />
          {error && <Message severity="error" text={error} className="mt-3" />}
          <Button label="התחבר" icon="pi pi-sign-in" className="mt-4" type="submit" />
        </form>
      </Card>
    </div>
  );
};

export default Login;
