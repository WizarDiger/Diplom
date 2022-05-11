import React, { useState, useEffect, useRef } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';


import Box from '@mui/material/Box';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';

import ChatInput from '../Layout/ChatInput';
import LeftMenu from '../Layout/LeftMenu';
import Footer from '../Layout/Footer';
import Header from '../Layout/Header';
const ChatPage = () => {
    const [chat, setChat] = useState([]);
    const latestChat = useRef(null);

    const [myData, setData] = useState("");
    latestChat.current = chat;
    const [myFriends, setFriends] = useState("");


    const getFriendsId = async () => {

            
        fetch('https://localhost:7049/api/ChatFriendId', {
            method: 'POST',
            credentials: 'include',
            headers:
            {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(
                {
                    Host: '9e0e7aa4-b2ce-481d-a463-551e2933a80d',
                    Friend: 1
                }
            )
        })
            .then(res => res.json())
            .then((result) => {

                setFriends(result);

            },
                (error) => {

                    alert(error);
                })
    }

    const getUserData = async () => {
        fetch('https://localhost:7049/api/Login', {
            method: 'GET',
            credentials: 'include',
        })
            .then((response) => response.json())
            .then((data) => {
                setData(data)
            });

    }
    useEffect(() => {
        getFriendsId();
        const connection = new HubConnectionBuilder()
            .withUrl('https://localhost:7049/hubs/chat')
            .withAutomaticReconnect()
            .build();

        connection.start()
            .then(result => {
                console.log('Connected!');

                connection.on('ReceiveMessage', message => {
                    const updatedChat = [...latestChat.current];
                    updatedChat.push(message);

                    setChat(updatedChat);
                });
            })
            .catch(e => console.log('Connection failed: ', e));

        getUserData();
    }, []);

    const sendMessage = async (user, message) => {
        const chatMessage = {
            user: user,
            message: message
        };

        try {
            await fetch('https://localhost:7049/api/Chat/messages', {
                method: 'POST',
                body: JSON.stringify(chatMessage),
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        }
        catch (e) {
            console.log('Sending message failed.', e);
        }
    }

    return (
        <>
            <Header />
            <div style={{ verticalAlign: 'top', width: '100%', marginTop: "0%", textAlign: 'start', display: 'flex', backgroundColor: 'whitesmoke' }}>

                <LeftMenu />
                <List style={{ width: '100%' }}>
                    <ListItem >

                        <ChatInput sendMessage={sendMessage} chat={chat} friendList={myFriends}/>
                    </ListItem>

                </List>
            </div>
            <Footer />
        </>
    );
};


export default ChatPage;