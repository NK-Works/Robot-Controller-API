--
-- PostgreSQL database dump
--

-- Dumped from database version 16.2
-- Dumped by pg_dump version 16.2

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: map; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.map (
    columns integer NOT NULL,
    rows integer NOT NULL,
    name character varying(50) NOT NULL,
    description character varying(800),
    created_date timestamp without time zone,
    modified_date timestamp without time zone,
    is_square boolean GENERATED ALWAYS AS (((rows > 0) AND (rows = columns))) STORED,
    id integer NOT NULL
);


ALTER TABLE public.map OWNER TO postgres;

--
-- Name: map_id_new_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.map_id_new_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.map_id_new_seq OWNER TO postgres;

--
-- Name: map_id_new_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.map_id_new_seq OWNED BY public.map.id;


--
-- Name: robot_command; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.robot_command (
    name character varying(50) NOT NULL,
    description character varying(800),
    is_move_command boolean NOT NULL,
    created_date timestamp without time zone,
    modified_date timestamp without time zone,
    id integer NOT NULL
);


ALTER TABLE public.robot_command OWNER TO postgres;

--
-- Name: robot_command_id_new_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.robot_command_id_new_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.robot_command_id_new_seq OWNER TO postgres;

--
-- Name: robot_command_id_new_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.robot_command_id_new_seq OWNED BY public.robot_command.id;


--
-- Name: user; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."user" (
    id integer NOT NULL,
    email character varying(255) NOT NULL,
    first_name character varying(100) NOT NULL,
    last_name character varying(100) NOT NULL,
    password_hash character varying(255) NOT NULL,
    description text,
    role character varying(50),
    created_date timestamp without time zone,
    modified_date timestamp without time zone
);


ALTER TABLE public."user" OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.user_id_seq OWNER TO postgres;

--
-- Name: user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_id_seq OWNED BY public."user".id;


--
-- Name: map id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.map ALTER COLUMN id SET DEFAULT nextval('public.map_id_new_seq'::regclass);


--
-- Name: robot_command id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.robot_command ALTER COLUMN id SET DEFAULT nextval('public.robot_command_id_new_seq'::regclass);


--
-- Name: user id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user" ALTER COLUMN id SET DEFAULT nextval('public.user_id_seq'::regclass);


--
-- Data for Name: map; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.map (columns, rows, name, description, created_date, modified_date, id) FROM stdin;
10	10	10x10 Map	Test Map	2024-03-02 00:45:22.415557	2024-03-02 00:45:22.415557	1
8	8	8x8 Map	A new map	2024-03-02 00:45:46.302432	2024-03-02 00:45:46.302432	2
20	15	20x15 Map	A non-square map	2024-03-02 00:47:37.936139	2024-03-02 00:47:37.936139	3
50	50	UpdatedVarMAP	A map using postman variable	2024-04-12 20:42:22.795778	2024-04-12 20:42:24.915946	40
50	50	50x50 Map	Map test run	2024-04-12 21:16:14.306566	2024-04-12 21:16:14.306683	41
\.


--
-- Data for Name: robot_command; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.robot_command (name, description, is_move_command, created_date, modified_date, id) FROM stdin;
LEFT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00	1
RIGHT	\N	t	2024-03-01 00:00:00	2024-03-01 00:00:00	2
MOVE	Move the robot forward	t	2024-03-01 00:00:00	2024-03-01 00:00:00	3
PLACE	Place the robot at a specific location	f	2024-03-01 00:00:00	2024-03-01 00:00:00	4
REPORT	Report the current state of the robot	f	2024-03-01 00:00:00	2024-03-01 00:00:00	5
\.


--
-- Data for Name: user; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."user" (id, email, first_name, last_name, password_hash, description, role, created_date, modified_date) FROM stdin;
6	anneshu123@gmail.com	Never	Know	$2a$11$1uJN8ZOxKLAvkReDiH32N.n/4nMAXjvr2lieUII08XMULobd.EKl2	\N	Admin	2024-04-03 14:16:07.629414	2024-04-03 14:16:07.630309
10	anneshunag@gmail.com	Anneshu	Nag	$2a$11$4VrLmfUGAG4kKVxyzkQz2uKLR2zE4f9UsLXuiB1dk6t19TGtSNTdW	\N	Admin	2024-04-06 17:59:43.75827	2024-04-06 17:59:43.758442
9	anneshutest_patch@gmail.com	Neverkk	Know	$2a$11$bnGHEfjsMmbB46OYIGfy5uJJ0Foy/CDRAAvdrAa81XnEvgdtwbW5S	\N	Admin	2024-04-06 16:40:12.190104	2024-04-06 16:40:12.190392
15				$2a$11$qkhoYSQADaG3MWYcUwFgBOceCfm1gauJL..Dfwb4YSFOSRjSYRcJG	\N		2024-04-12 21:25:01.797161	2024-04-12 21:25:01.797163
22	aeshutest@gmail.com	Ann	Nag	$2a$11$H1x20YxLkOOu5mpTT3NBQuXEcEIuPsQrYnQY8l35kERGa/zEJfpya	\N	Admin	2024-04-13 00:08:07.521126	2024-04-13 00:08:07.537803
\.


--
-- Name: map_id_new_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.map_id_new_seq', 5, true);


--
-- Name: robot_command_id_new_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.robot_command_id_new_seq', 5, true);


--
-- Name: user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_id_seq', 22, true);


--
-- Name: map map_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.map
    ADD CONSTRAINT map_pkey PRIMARY KEY (id);


--
-- Name: robot_command robot_command_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.robot_command
    ADD CONSTRAINT robot_command_pkey PRIMARY KEY (id);


--
-- Name: user user_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."user"
    ADD CONSTRAINT user_pkey PRIMARY KEY (id);


--
-- PostgreSQL database dump complete
--

