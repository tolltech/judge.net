﻿import React, {useState} from "react";
import {LockOutlined, MailOutlined} from '@ant-design/icons';
import {Button, Col, Form, Input, Row} from "antd";
import {judgeApi} from "../api/JudgeApi.ts";
import {useNavigate} from "react-router-dom";
import {handleError} from "../helpers/handleError.ts";
import {Typography} from 'antd';

const {Text} = Typography;

export const Login: React.FC = () => {
    const navigate = useNavigate();
    const [isLoading, setLoading] = useState<boolean>();
    const [errors, setErrors] = useState<React.ReactNode[]>();
    const onFinish = async (values: any) => {
        setLoading(true);
        const api = judgeApi();
        try {
            const token = await api.api.loginTokenCreate({email: values.email, password: values.password});
            localStorage.setItem("token", token.data.token);

            navigate("/problems");
        } catch (e: any) {
            setLoading(false);

            const status = e.response.status;

            if (status === 404) {
                setErrors([<Text type={"danger"}>User not found</Text>]);
            } else if (status === 400) {
                setErrors([<Text type={"danger"}>Invalid password</Text>]);
            } else {
                handleError(e);
            }
        }
    };
    return (
        <Row justify="center" align="middle">
            <Col span={6}>
                <Form
                    name="normal_login"
                    className="login-form"
                    initialValues={{remember: true}}
                    onFinish={onFinish}
                    layout="vertical"
                >
                    <Form.Item
                        name="email"
                        rules={[{required: true, message: 'Please input your Email!'}]}
                    >
                        <Input prefix={<MailOutlined className="site-form-item-icon"/>} placeholder="Email"/>
                    </Form.Item>
                    <Form.Item
                        name="password"
                        rules={[{required: true, message: 'Please input your Password!'}]}
                    >
                        <Input
                            prefix={<LockOutlined className="site-form-item-icon"/>}
                            type="password"
                            placeholder="Password"
                        />
                    </Form.Item>
                    <Form.Item>
                        <Button type="primary" loading={isLoading} htmlType="submit" className="login-form-button">
                            Log in
                        </Button>
                    </Form.Item>
                    <Form.ErrorList errors={errors}/>
                </Form>
            </Col>
        </Row>
    );
};